export interface UserModel {
  id?: number;
  fullName?: string;
  photoUrl?: string;
  isTyping?: boolean;
}

export interface UserTokenModel {
  accessToken: string;
  refreshToken: string;
}
