import { AfterViewChecked, Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { HubConstants } from '../Constants';
import { NotificationMessage } from '../models/NotificationMessage';

@Component({
  selector: 'app-serverless',
  templateUrl: './serverless.component.html',
  styleUrls: ['./serverless.component.css'],
})
export class ServerlessComponent implements OnInit, AfterViewChecked {
  private hubConnection: HubConnection | undefined;

  messages: NotificationMessage[] = [];

  hubConnectionState = signalR.HubConnectionState;
  currentHubConnectionState = this.hubConnectionState.Disconnected;
  baseStationId: string = null;
  connectedBaseStationId: string = null;
  connectedClientCount = null;
  connectionPossible = true;

  constructor() {}

  ngAfterViewChecked(): void {
    this.currentHubConnectionState = this.hubConnection.state;
  }

  ngOnInit() {
    this.connectToSignalR();
  }

  stopConnection() {
    this.hubConnection &&
      this.hubConnection.stop().then(() => {
        this.baseStationId = null;
        this.connectedBaseStationId = null;
        this.connectedClientCount = null;
      });
  }

  connectToSignalR() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.serverlessServerURL)
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {})
      .catch((err) => {
        console.error(err.toString());
        this.connectionPossible = false;
      });

    this.hubConnection.on(
      HubConstants.NOTIFICATION_CREATED_HUB_EVENT,
      (data: NotificationMessage) => {
        data.id = this.messages.length + 1;
        this.messages.push(data);
        console.log(data);
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
        this.connectionPossible = false;
      });
    }

    if (!this.hubConnection) {
      this.connectToSignalR();
    }
  }

  CleanAll(id: number = null) {
    if (id) {
      const index = this.messages.findIndex((x) => x.id == id);
      if (index > -1) {
        this.messages.splice(index, 1);
      }
    } else this.messages = [];
  }
}
