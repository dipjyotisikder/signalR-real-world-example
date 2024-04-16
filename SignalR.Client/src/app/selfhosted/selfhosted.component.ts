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

  constructor() {}

  ngOnInit() {}
}
