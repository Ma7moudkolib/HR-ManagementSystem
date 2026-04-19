import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { UserForAuthDto } from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class MockAuthService {
  private isLoggedInStatus = false;
  
  login(credentials: UserForAuthDto): Observable<{success: boolean, token?: string}> {
    if(credentials.userName === 'admin' && credentials.password === 'password123') {
      this.isLoggedInStatus = true;
      localStorage.setItem('auth_token', 'mock.jwt.token.123');
      return of({ success: true, token: 'mock.jwt.token.123' });
    }
    return of({ success: false });
  }

  isLoggedIn(): boolean {
    return this.isLoggedInStatus || !!localStorage.getItem('auth_token');
  }

  logout() {
    this.isLoggedInStatus = false;
    localStorage.removeItem('auth_token');
  }
}
