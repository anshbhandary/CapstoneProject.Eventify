// auth.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _isLoggedIn = new BehaviorSubject<boolean>(this.getIsLoggedInFromSession());
  isLoggedIn$ = this._isLoggedIn.asObservable();

  constructor() {}

  private getIsLoggedInFromSession(): boolean {
    return sessionStorage.getItem('isLogged') === 'true';
  }

  login(userRole: string): void {
    sessionStorage.setItem('isLogged', 'true');
    localStorage.setItem('userRole', userRole);
    this._isLoggedIn.next(true);
  }

  logout(): void {
    sessionStorage.clear();
    localStorage.removeItem('userRole');
    this._isLoggedIn.next(false);
  }

  getUserRole(): string | null {
    return localStorage.getItem('userRole');
  }
}
