import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { selfHostedConstants } from '../constants/selfhosted-constants';
import * as signalR from '@microsoft/signalr';
import { AuthService } from './auth.service';
import { NotificationMessage } from '../models/NotificationMessage';
import { HubConstants } from '../Constants';
import { MessageModel } from '../models/MessageModel';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HubService {
  public hubConnection: signalR.HubConnection | null = null;

  private messageIsCreatedSubject = new BehaviorSubject<MessageModel | null>(
    null
  );

  private userIsOnlineSubject = new BehaviorSubject<boolean | null>(null);

  constructor(private authService: AuthService) {}

  listenMessageIsCreatedEvent() {
    return this.messageIsCreatedSubject.asObservable();
  }

  listenUserIsOnlineEvent() {
    return this.messageIsCreatedSubject.asObservable();
  }

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
      .then(() => {})
      .catch(() => {});

    this.hubConnection.on(
      HubConstants.serverEvents.MESSAGE_IS_CREATED,
      (message: MessageModel) => {
        this.messageIsCreatedSubject.next(message);
      }
    );

    this.hubConnection.on(
      HubConstants.serverEvents.USER_IS_ONLINE,
      (message: boolean) => {
        this.userIsOnlineSubject.next(message);
      }
    );
  }

  stopHub() {
    this.hubConnection && this.hubConnection.stop().then(() => {});
  }
}
