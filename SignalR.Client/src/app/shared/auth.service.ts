import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { selfHostedConstants } from '../constants/selfhosted-constants';
import { environment } from 'src/environments/environment';
import { UserTokenModel } from '../models/UserModel';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private accessTokenKey = 'access_token';
  private refreshTokenKey = 'refresh_token';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(
    this.hasToken()
  );

  constructor(private http: HttpClient) {}

  public hasToken(): boolean {
    return !!localStorage.getItem(this.accessTokenKey);
  }

  public getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  public setToken(token: string, refreshToken: string): void {
    localStorage.setItem(this.accessTokenKey, token);
    localStorage.setItem(this.refreshTokenKey, refreshToken);
    this.isAuthenticatedSubject.next(true);
  }

  public removeToken(): void {
    localStorage.removeItem(this.accessTokenKey);
    this.isAuthenticatedSubject.next(false);
  }

  //   logout(): Observable<any> {
  //     return this.http.post<any>(`${this.apiUrl}/logout`, {}).pipe(
  //       tap(() => {
  //         this.removeToken();
  //       }),
  //       catchError((error) => {
  //         return throwError(() => new Error(error));
  //       })
  //     );
  //   }

  isAuthenticated(): Observable<boolean> {
    return this.isAuthenticatedSubject.asObservable();
  }

  getTokenValue(): string | null {
    return this.getAccessToken();
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
        {}
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

// const getAuthHeaders = () => {
//   return {
//     Authorization: `Bearer ${authService.getToken()?.accessToken}`,
//   };
// };

// class CustomHttpClient extends signalR.DefaultHttpClient {
//   constructor() {
//     super(console);
//   }

//   public override async send(
//     request: signalR.HttpRequest
//   ): Promise<signalR.HttpResponse> {
//     const authHeaders = getAuthHeaders();
//     request.headers = { ...request.headers, ...authHeaders };

//     try {
//       const response = await super.send(request);
//       return response;
//     } catch (er) {
//       if (er instanceof signalR.HttpError) {
//         const error = er as signalR.HttpError;
//         if (error.statusCode == 401) {
//           //token expired - trying a refresh via refresh token
//           await authService.refresh();
//           const authHeaders = getAuthHeaders();
//           request.headers = { ...request.headers, ...authHeaders };
//         }
//       } else {
//         throw er;
//       }
//     }
//     //re try the request
//     return super.send(request);
//   }
// }
