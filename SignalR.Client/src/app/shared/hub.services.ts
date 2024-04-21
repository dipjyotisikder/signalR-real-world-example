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
        }
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
    return this.hubConnection?.invoke(
      HubConstants.serverEvents.USER_IS_TYPING,
      conversationId,
      isTyping
    );
  }
}
