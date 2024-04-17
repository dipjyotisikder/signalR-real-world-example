export interface UserModel {
  id?: number;
  fullName?: string;
  photoUrl?: string;
  onLine?: string;
}

export interface UserTokenModel {
  accessToken: string;
  refreshToken: string;
}
