import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { selfHostedConstants } from '../constants/selfhosted-constants';
import * as signalR from '@microsoft/signalr';
import { AuthService } from './auth.service';
import { NotificationMessage } from '../models/NotificationMessage';
import { HubConstants } from '../Constants';
import { MessageModel } from '../models/MessageModel';
import { BehaviorSubject } from 'rxjs';
import { UserModel } from '../models/UserModel';

@Injectable({
  providedIn: 'root',
})
export class HubService {
  public hubConnection: signalR.HubConnection | null = null;
  private messageIsCreatedEvent = new BehaviorSubject<MessageModel | null>(
    null
  );

  private userIsOnlineEvent = new BehaviorSubject<boolean | null>(null);
  private userIsJoinedEvent = new BehaviorSubject<UserModel | null>(null);
  private userIsTypingEvent = new BehaviorSubject<UserModel | null>(null);

  constructor(private authService: AuthService) { }

  startHub() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(
        environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.SIGNALR_ENDPOINT,
        <signalR.IHttpConnectionOptions>{
          accessTokenFactory: () => this.authService.getAccessToken(),
          httpClient: new CustomHttpClient(this.authService)
        },
      )
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => { })
      .catch(() => { });

    this.hubConnection.on(
      HubConstants.serverEvents.MESSAGE_IS_CREATED,
      (message: MessageModel) => {
        this.messageIsCreatedEvent.next(message);
      }
    );

    this.hubConnection.on(
      HubConstants.serverEvents.USER_IS_ONLINE,
      (message: boolean) => {
        this.userIsOnlineEvent.next(message);
      }
    );

    this.hubConnection.on(
      HubConstants.serverEvents.USER_IS_JOINED,
      (user: UserModel) => {
        this.userIsJoinedEvent.next(user);
      }
    );

    this.hubConnection.on(
      HubConstants.serverEvents.USER_IS_TYPING,
      (user: UserModel) => {
        this.userIsTypingEvent.next(user);
      }
    );
  }

  stopHub() {
    this.hubConnection && this.hubConnection.stop().then(() => { });
  }

  messageIsCreatedEventHandler() {
    return this.messageIsCreatedEvent.asObservable();
  }

  userIsOnlineEventHandler() {
    return this.userIsOnlineEvent.asObservable();
  }

  userIsJoinedEventHandler() {
    return this.userIsJoinedEvent.asObservable();
  }

  userIsTypingEventHandler() {
    return this.userIsTypingEvent.asObservable();
  }

  triggerUserIsTypingEvent(conversationId: number, isTyping: boolean) {
    if (this.hubConnection?.state != signalR.HubConnectionState.Connected) {
      this.startHub();
    }

    return this.hubConnection?.invoke(
      HubConstants.serverEvents.USER_IS_TYPING,
      conversationId,
      isTyping
    );
  }
}

class CustomHttpClient extends signalR.DefaultHttpClient {
  constructor(private authService: AuthService) {
    super(console);
  }

  public override async send(request: signalR.HttpRequest): Promise<signalR.HttpResponse> {
    this.addTokenToRequest(request);

    try {
      return await super.send(request);
    } catch (er) {
      if (er instanceof signalR.HttpError) {
        const error = er as signalR.HttpError;
        if (error.statusCode == 401) {
          await this.authService.refreshToken();
          this.addTokenToRequest(request);
        }
      } else {
        throw er;
      }
    }

    return super.send(request);
  }

  private addTokenToRequest(request: signalR.HttpRequest): signalR.HttpRequest {
    request.content = request.content;

    const accessToken = this.authService.getAccessToken();
    if (accessToken) {
      request.headers = { ...request.headers, ...{ Authorization: `Bearer ${accessToken}` } }
    }

    return request;
  }
}