import { ConversationModel } from './ConversationModel';

export interface MessageModel {
  id: number;
  text: string;
  conversation?: ConversationModel;
  createdAt: Date;
}

export interface MessageCreateModel {
  conversationId: number;
  text: string;
}
