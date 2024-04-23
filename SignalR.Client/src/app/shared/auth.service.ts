import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { selfHostedConstants } from '../constants/selfhosted-constants';
import { environment } from 'src/environments/environment';
import { UserTokenModel } from '../models/UserModel';
import { JwtPayload, jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private accessTokenKey = 'access_token';
  private refreshTokenKey = 'refresh_token';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(
    this.hasToken()
  );

  constructor(private http: HttpClient) { }

  public hasToken(): boolean {
    return !!localStorage.getItem(this.accessTokenKey);
  }

  public getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  public getRefreshToken = () => {
    return localStorage.getItem(this.refreshTokenKey);
  }

  public setToken(token: string, refreshToken: string): void {
    localStorage.setItem(this.accessTokenKey, token);
    localStorage.setItem(this.refreshTokenKey, refreshToken);
    this.isAuthenticatedSubject.next(true);
  }

  public removeToken(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    this.isAuthenticatedSubject.next(false);
  }

  public currentUserId(): any {
    const accessToken = this.getAccessToken();
    if (accessToken) {
      const decodedToken = jwtDecode<CustomJwtPayload>(accessToken);

      return decodedToken[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
      ];
    }
    return null;
  }

  isAuthenticated(): Observable<boolean> {
    return this.isAuthenticatedSubject.asObservable();
  }

  refreshToken(): Observable<UserTokenModel> {
    const token = this.getAccessToken();
    if (!token) {
      return throwError(() => new Error('Token not found'));
    }

    return this.http
      .post<UserTokenModel>(
        environment.selfHostedServerURL +
        '/' +
        selfHostedConstants.REFRESH_TOKEN_ENDPOINT,
        {
          refreshToken: this.getRefreshToken()
        }
      )
      .pipe(
        tap((response) => {
          if (response) {
            this.setToken(response.accessToken, response.refreshToken);
          }
        }),
        catchError((error) => {
          return throwError(() => new Error(error));
        })
      );
  }
}

export interface CustomJwtPayload extends JwtPayload {
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name': string;
}
