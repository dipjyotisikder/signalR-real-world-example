import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConstants, MessageConstants } from '../Constants';
import { NotificationMessage } from '../models/NotificationMessage';
import { SelfHostedService } from './selfhosted.services';

@Component({
  selector: 'app-selfhosted',
  templateUrl: './selfhosted.component.html',
  styleUrls: ['./selfhosted.component.css'],
})
export class SelfHostedComponent implements OnInit {
  // HUB CONFIGURATION
  hubConnection: signalR.HubConnection | null = null;
  hubConnectionState = signalR.HubConnectionState;

  messages: NotificationMessage[] = new Array<any>();

  isConnected: boolean = false;
  connectedGroups: string[] = [];
  availableGroups: string[] = [];
  connectedClientCount: number | null = null;

  constructor(
    public cdr: ChangeDetectorRef,
    public selfHostedService: SelfHostedService
  ) {}

  ngOnInit() {
    // this.selfHostedService.getGroups().subscribe((success) => {
    //   this.availableGroups = success;
    // });

    this.connectToSignalR();
  }

  connectToSignalR() {
    this.hubConnection = this.selfHostedService.buildHubConnection();
    this.hubConnection
      .start()
      .then(() => {
        this.setIsConnected();
        this.cdr.detectChanges();
      })
      .catch(() => {
        this.setIsConnected();
        window.alert(MessageConstants.CONNECTION_FAILED);
      });

    this.hubConnection.on(
      HubConstants.NOTIFICATION_CREATED_HUB_EVENT,
      (data: NotificationMessage) => {
        data.id = this.messages.length + 1;
        this.messages.push(data);
        this.cdr.detectChanges();
      }
    );

    this.hubConnection.on(
      HubConstants.CONNECTED_CLIENT_UPDATED_HUB_EVENT,
      (data: number) => {
        console.log('connection count', data);
        this.connectedClientCount = data;
        this.cdr.detectChanges();
      }
    );

    this.hubConnection.on(
      HubConstants.EXCEPTION_OCCURRED_HUB_EVENT,
      (data: string) => {
        console.log(data);
      }
    );
  }

  stopConnection() {
    this.hubConnection &&
      this.hubConnection.stop().then(() => {
        this.connectedGroups = [];
        this.connectedClientCount = null;
        this.isConnected = false;
        this.cdr.detectChanges();
      });
  }

  joinGroup(groupName: string) {
    if (
      this.connectedGroups &&
      this.connectedGroups.some((x) => x == groupName)
    )
      return;

    if (
      this.hubConnection &&
      this.hubConnection.state !== signalR.HubConnectionState.Connected
    ) {
      this.setIsConnected();
      window.alert(MessageConstants.PLEASE_CONNECT_ALERT);
      return;
    }

    this.hubConnection &&
      this.hubConnection
        .invoke(HubConstants.JOIN_GROUP_HUB_METHOD, groupName)
        .then(() => {
          this.connectedGroups.push(groupName);
          this.cdr.detectChanges();
        });
  }

  cleanAll(id?: number) {
    if (id) {
      const index = this.messages.findIndex((x) => x.id == id);
      if (index > -1) {
        this.messages.splice(index, 1);
      }
    } else this.messages = [];

    this.cdr.detectChanges();
  }

  setIsConnected() {
    this.isConnected = this.hubConnection
      ? this.hubConnection.state == signalR.HubConnectionState.Connected
      : false;
  }
}
