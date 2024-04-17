import { UserModel } from './UserModel';

export interface MessageModel {
  id: number;
  text: string;
  creatorUser?: UserModel;
  createdAt: string;
}

export interface MessageCreateModel {
  conversationId: number;
  text: string;
}
