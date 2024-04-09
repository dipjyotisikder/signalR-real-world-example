import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { selfHostedConstants } from '../constants/selfhosted-constants';
import * as signalR from '@microsoft/signalr';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root',
})
export class SelfHostedService {
  constructor(private http: HttpClient) {}

  getGroups() {
    return this.http.get<string[]>(
      environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.CREATE_GROUP_ENDPOINT
    );
  }

  getUsers() {
    return this.http.get<User[]>(
      environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.GET_USERS_ENDPOINT
    );
  }

  buildHubConnection(): signalR.HubConnection {
    return new signalR.HubConnectionBuilder()
      .withUrl(
        environment.selfHostedServerURL +
          '/' +
          selfHostedConstants.SIGNALR_ENDPOINT
      )
      .withAutomaticReconnect()
      .build();
  }

  avatarUrls: string[] = [
    'https://bootdey.com/img/Content/avatar/avatar1.png',
    'https://bootdey.com/img/Content/avatar/avatar2.png',
    'https://bootdey.com/img/Content/avatar/avatar3.png',
    'https://bootdey.com/img/Content/avatar/avatar7.png',
    'https://bootdey.com/img/Content/avatar/avatar8.png',
  ];

  getRandomAvatarUrl(): string {
    const randomIndex = Math.floor(Math.random() * this.avatarUrls.length);
    return this.avatarUrls[randomIndex];
  }
}
