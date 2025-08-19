import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoggedInUser } from '../_models/logged-in-user';
import { LoginForm } from '../_models/login-form';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  currentUser = signal<LoggedInUser | null>(null);

  login(model: LoginForm) {
    return this.http.post<LoggedInUser>(this.baseUrl + "/account/login", model).pipe(map(
      user => {
        if(user) this.setCurrentUser(user)
        return user;
      }
    ));
  }

  register(model: LoginForm) {
    return this.http.post<LoggedInUser>(this.baseUrl + "/account/register", model).pipe(map(
      user => {
        if(user) this.setCurrentUser(user)
        return user;
     }
    ));
  }

  setCurrentUser(user: LoggedInUser) {
    localStorage.setItem("user", JSON.stringify(user));
    this.currentUser.set(user);
  }

  unsetCurrentUser() {
    localStorage.removeItem("user");
    this.currentUser.set(null);
  }
}
