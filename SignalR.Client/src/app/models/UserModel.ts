export interface UserModel {
  id?: number;
  fullName?: string;
  photoUrl?: string;
  onLine?: string;
}

export interface UserTokenModel {
  fullName: string;
  accessToken: string;
}
