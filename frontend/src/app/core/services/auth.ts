import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private apiUrl = '/api/auth';
  private tokenKey = 'sessionToken';

  constructor(private http: HttpClient) {}

  requestCode(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/request-link`, {});
  }

  verifyCode(code: string): Observable<{ sessionToken: string }> {
    return this.http
      .get<{ sessionToken: string }>(`${this.apiUrl}/verify?token=${code}`)
      .pipe(tap((response) => this.setToken(response.sessionToken)));
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }
}
