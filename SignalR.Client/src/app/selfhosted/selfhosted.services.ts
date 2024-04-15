import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { selfHostedConstants } from '../constants/selfhosted-constants';
import * as signalR from '@microsoft/signalr';
import { UserModel } from '../models/UserModel';
import {
  ConversationCreateModel,
  ConversationModel,
} from '../models/ConversationModel';
import { MessageCreateModel, MessageModel } from '../models/MessageModel';

@Injectable({
  providedIn: 'root',
})
export class SelfHostedService {
  constructor(private http: HttpClient) {}

  getGroups() {
    return this.http.get<string[]>(
      environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.CREATE_CONVERSATION_ENDPOINT
    );
  }

  getUsers() {
    return this.http.get<UserModel[]>(
      environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.GET_USERS_ENDPOINT
    );
  }

  createUser(user: UserModel) {
    return this.http.post<UserModel>(
      environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.CREATE_USERS_ENDPOINT,
      user
    );
  }

  getConversations() {
    return this.http.get<ConversationModel[]>(
      environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.GET_CONVERSATIONS_ENDPOINT
    );
  }

  createConversation(payload: ConversationCreateModel) {
    return this.http.post<ConversationModel>(
      environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.GET_CONVERSATIONS_ENDPOINT,
      payload
    );
  }

  getMessages(conversationId: number) {
    return this.http.get<MessageModel[]>(
      environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.GET_MESSAGES_ENDPOINT(conversationId)
    );
  }

  createMessage(payload: MessageCreateModel) {
    return this.http.post<MessageModel>(
      environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.CREATE_MESSAGE_ENDPOINT(payload.conversationId),
      payload
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
