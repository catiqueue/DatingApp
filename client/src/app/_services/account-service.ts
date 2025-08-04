import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoggedInUser } from '../_models/logged-in-user';
import { LoginForm } from '../_models/login-form';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = "https://localhost:5001/api";
  currentUser = signal<LoggedInUser | null>(null);

  login(model: LoginForm) {
    return this.http.post<LoggedInUser>(this.baseUrl + "/account/login", model).pipe(map(
      user => {
        if(user) {
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUser.set(user);
        }
        return user;
      }
    ));
  }

  register(model: LoginForm) {
    return this.http.post<LoggedInUser>(this.baseUrl + "/account/register", model).pipe(map(
      user => {
        if(user) {
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUser.set(user);
        }
        return user;
     }
    ));
  }

  logout() {
    localStorage.removeItem("user");
    this.currentUser.set(null);
  }
}
