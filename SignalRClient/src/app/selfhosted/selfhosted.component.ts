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
export class SelfhostedComponent implements OnInit {
  hubConnection: signalR.HubConnection | undefined;
  messages: NotificationMessage[] = new Array<any>();

  hubConnectionState = signalR.HubConnectionState;

  group: string = null;
  connectedGroup: string = null;
  connectedGroups: string[] = [];
  connectedClientCount = null;
  currentHubConnectionState = this.hubConnectionState.Disconnected;

  constructor() {}
  ngAfterViewChecked(): void {
    this.currentHubConnectionState = this.hubConnection.state;
  }

  ngOnInit() {
    this.connectToSignalR();
  }

  stopConnection() {
    this.hubConnection.stop().then((x) => {
      this.group = null;
      this.connectedGroup = null;
      this.connectedGroups = [];
      this.connectedClientCount = null;
    });
  }

  connectToSignalR() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.selfHostedServerURL)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((err) => {
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
      HubConstants.EXCEPTION_OCCURED_HUB_EVENT,
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
      this.hubConnection.start().catch((err) => {
        console.error(err.toString());
        window.alert(MessageConstants.CONNECTION_FAILED);
      });
    }

    if (!this.hubConnection) {
      this.connectToSignalR();
    }
  }

  joinGroup() {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      window.alert(MessageConstants.PLEASE_CONNECT_ALERT);
      return;
    }

    this.hubConnection
      .invoke(HubConstants.JOIN_GROUP_HUB_METHOD, this.group)
      .then(() => {
        this.connectedGroup = this.group;
        this.connectedGroups.push(this.group);
        this.group = null;
      });
  }

  cleanAll(id: number = null) {
    if (id) {
      const index = this.messages.findIndex((x) => x.id == id);
      if (index > -1) {
        this.messages.splice(index, 1);
      }
    } else this.messages = [];
  }
}
