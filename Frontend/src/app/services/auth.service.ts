import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { UserForAuthDto, UserForRegistrationDto } from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiBaseUrl}/authentication`;
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.checkAuthStatus());

  constructor(private http: HttpClient) {}

  login(credentials: UserForAuthDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        if (response && response.token) {
          localStorage.setItem('auth_token', response.token);
          this.isLoggedInSubject.next(true);
        }
      })
    );
  }

  register(userData: UserForRegistrationDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, userData).pipe(
      tap(response => {
        if (response && response.token) {
          localStorage.setItem('auth_token', response.token);
          this.isLoggedInSubject.next(true);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    this.isLoggedInSubject.next(false);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('auth_token');
  }

  getIsLoggedIn(): Observable<boolean> {
    return this.isLoggedInSubject.asObservable();
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  private checkAuthStatus(): boolean {
    return !!localStorage.getItem('auth_token');
  }
}
