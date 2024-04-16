import { UserModel } from './UserModel';

export interface ConversationModel {
  id: number;
  title: string;
  creatorUser?: UserModel;
  createdAt: Date;
}

export interface ConversationCreateModel {
  title: string;
  creatorUserId: number;
}

export interface ConversationAudienceModel extends ConversationModel {
  audienceUsers: UserModel[];
}
