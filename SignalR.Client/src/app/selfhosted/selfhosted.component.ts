import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { HubConstants, MessageConstants } from '../Constants';
import { NotificationMessage } from '../models/NotificationMessage';

@Component({
  selector: 'app-selfhosted',
  templateUrl: './selfhosted.component.html',
  styleUrls: ['./selfhosted.component.css'],
})
export class SelfHostedComponent implements OnInit {
  // HUB CONFIGURATION
  hubConnection: signalR.HubConnection | null = null;
  hubConnectionState = signalR.HubConnectionState;
  currentHubConnectionState = signalR.HubConnectionState.Disconnected;

  messages: NotificationMessage[] = new Array<any>();

  groupName: string = '';
  connectedGroup: string = '';
  connectedGroups: string[] = [];
  connectedClientCount: number = 0;

  constructor() {}

  ngAfterViewChecked(): void {
    this.currentHubConnectionState = this.hubConnection
      ? this.hubConnection.state
      : signalR.HubConnectionState.Disconnected;
  }

  ngOnInit() {
    this.connectToSignalR();
  }

  stopConnection() {
    this.hubConnection &&
      this.hubConnection.stop().then(() => {
        this.groupName = '';
        this.connectedGroup = '';
        this.connectedGroups = [];
        this.connectedClientCount = 0;
      });
  }

  connectToSignalR() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.selfHostedServerURL)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(() => {
      window.alert(MessageConstants.CONNECTION_FAILED);
    });

    this.hubConnection.on(
      HubConstants.NOTIFICATION_CREATED_HUB_EVENT,
      (data: NotificationMessage) => {
        data.id = this.messages.length + 1;
        this.messages.push(data);
      }
    );

    this.hubConnection.on(
      HubConstants.CONNECTED_CLIENT_UPDATED_HUB_EVENT,
      (data: number) => {
        this.connectedClientCount = data;
      }
    );

    this.hubConnection.on(
      HubConstants.EXCEPTION_OCCURRED_HUB_EVENT,
      (data: string) => {
        console.log(data);
      }
    );
  }

  reconnect() {
    if (
      this.hubConnection &&
      this.hubConnection.state == signalR.HubConnectionState.Disconnected
    ) {
      this.hubConnection.start().catch((err: any) => {
        console.error(err.toString());
        window.alert(MessageConstants.CONNECTION_FAILED);
      });
    }

    if (!this.hubConnection) {
      this.connectToSignalR();
    }
  }

  joinGroup() {
    if (
      this.hubConnection &&
      this.hubConnection.state !== signalR.HubConnectionState.Connected
    ) {
      window.alert(MessageConstants.PLEASE_CONNECT_ALERT);
      return;
    }

    this.hubConnection &&
      this.hubConnection
        .invoke(HubConstants.JOIN_GROUP_HUB_METHOD, this.groupName)
        .then(() => {
          this.connectedGroup = this.groupName;
          this.connectedGroups.push(this.groupName);
          this.groupName = '';
        });
  }

  cleanAll(id?: number) {
    if (id) {
      const index = this.messages.findIndex((x) => x.id == id);
      if (index > -1) {
        this.messages.splice(index, 1);
      }
    } else this.messages = [];
  }
}
