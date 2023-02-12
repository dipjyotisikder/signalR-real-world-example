import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import * as uuid from 'uuid';

@Component({
  selector: 'app-selfhosted',
  templateUrl: './selfhosted.component.html',
  styleUrls: ['./selfhosted.component.css'],
})
export class SelfhostedComponent implements OnInit {
  private hubConnection: signalR.HubConnection | undefined;

  messages: any[] = new Array<any>();

  userId: string;

  signalR = {
    serverUrl: 'https://localhost:44333/signalR/notificationHub',
  };

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
    this.userId = uuid.v4();
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
      .withUrl(this.signalR.serverUrl)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((err) => {
      console.error(err.toString());
      window.alert('Could not connect!');
    });

    this.hubConnection.on('NotificationCreated', (data: any) => {
      data.id = this.messages.length + 1;
      this.messages.push(data);
      this.messages = JSON.parse(JSON.stringify(this.messages));
      console.log(data);
    });

    this.hubConnection.on('ConnectedClientUpdated', (data: number) => {
      this.connectedClientCount = JSON.parse(JSON.stringify(data));
    });

    this.hubConnection.on('ExceptionOccured', (data: string) => {
      console.log(data);
    });
  }

  reconnect() {
    if (
      this.hubConnection &&
      this.hubConnection.state == signalR.HubConnectionState.Disconnected
    ) {
      this.hubConnection.start().catch((err) => {
        console.error(err.toString());
        window.alert('Could not connect!');
      });
    }

    if (!this.hubConnection) {
      this.connectToSignalR();
    }
  }

  joinGroup() {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      window.alert('Please connect first!');
      return;
    }

    this.hubConnection.invoke('JoinGroup', this.group).then(() => {
      this.connectedGroup = this.group;
      this.connectedGroups.push(this.group);
      this.group = null;
      console.log(this.connectedGroups);
    });
  }

  CleanAll(id: any = null) {
    if (id) {
      const index = this.messages.findIndex((x) => x.id == id);
      if (index > -1) {
        this.messages.splice(index, 1);
      }
    } else this.messages = [];
  }
}
