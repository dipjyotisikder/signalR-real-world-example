import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.selfHostedServerURL;
  private authTokenKey = 'auth_token';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(
    this.hasToken()
  );

  constructor(private http: HttpClient) {}

  private hasToken(): boolean {
    return !!localStorage.getItem(this.authTokenKey);
  }

  public getToken(): string | null {
    return localStorage.getItem(this.authTokenKey);
  }

  public setToken(token: string): void {
    localStorage.setItem(this.authTokenKey, token);
    this.isAuthenticatedSubject.next(true);
  }

  public removeToken(): void {
    localStorage.removeItem(this.authTokenKey);
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
    return this.getToken();
  }

  //   refreshToken(): Observable<string> {
  //     const token = this.getToken();
  //     if (!token) {
  //       return throwError(() => new Error('Token not found'));
  //     }

  //     return this.http.post<any>(`${this.apiUrl}/refresh-token`, { token }).pipe(
  //       tap((response) => {
  //         if (response && response.newToken) {
  //           this.setToken(response.newToken);
  //         }
  //       }),
  //       catchError((error) => {
  //         return throwError(() => new Error(error));
  //       })
  //     );
  //   }
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
