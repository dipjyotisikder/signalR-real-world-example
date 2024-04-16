import { ConversationModel } from './ConversationModel';
import { UserModel } from './UserModel';

export interface MessageModel {
  id: number;
  text: string;
  creatorUser?: UserModel;
  createdAt: Date;
}

export interface MessageCreateModel {
  conversationId: number;
  text: string;
}
