import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { selfHostedConstants } from '../constants/selfhosted-constants';
import * as signalR from '@microsoft/signalr';
import { AuthService } from './auth.service';
import { NotificationMessage } from '../models/NotificationMessage';
import { HubConstants } from '../Constants';
import { MessageModel } from '../models/MessageModel';

@Injectable({
  providedIn: 'root',
})
export class HubService {
  public hubConnection: signalR.HubConnection | null = null;

  constructor(private authService: AuthService) {}

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

    // this.hubConnection.on(
    //   HubConstants.NOTIFICATION_CREATED_HUB_EVENT,
    //   (data: NotificationMessage) => {}
    // );

    // this.hubConnection.on(
    //   HubConstants.CONNECTED_CLIENT_UPDATED_HUB_EVENT,
    //   (data: number) => {
    //     console.log('connection count', data);
    //   }
    // );

    // this.hubConnection.on(
    //   HubConstants.EXCEPTION_OCCURRED_HUB_EVENT,
    //   (data: string) => {
    //     console.log(data);
    //   }
    // );

    this.hubConnection.on(
      HubConstants.serverEvents.MESSAGE_IS_CREATED,
      (message: MessageModel) => {
        console.log('server event data', message);
      }
    );

    this.hubConnection.on(
      HubConstants.serverEvents.USER_IS_ONLINE,
      (message: boolean) => {
        console.log('server event data', message);
      }
    );
  }

  stopHub() {
    this.hubConnection && this.hubConnection.stop().then(() => {});
  }
}
