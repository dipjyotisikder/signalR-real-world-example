import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.selfHostedServerURL; //'https://your-api-url.com'; // Replace this with your .NET API base URL
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

  // Example function to make authenticated requests
  //   getData(): Observable<any> {
  //     const token = this.getToken();
  //     if (!token) {
  //       return throwError(() => new Error('Token not found'));
  //     }

  //     return this.http
  //       .get<any>(`${this.apiUrl}/data`, {
  //         headers: { Authorization: `Bearer ${token}` },
  //       })
  //       .pipe(
  //         catchError((error) => {
  //           return throwError(() => new Error(error));
  //         })
  //       );
  //   }
}
