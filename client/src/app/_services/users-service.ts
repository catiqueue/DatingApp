import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getUsers() {
    return this.http.get<User[]>(this.baseUrl + "/users/all");
  }

  getUserByUsername(username: string) {
    return this.http.get<User>(this.baseUrl + "/users/" + username);
  }

  getUserById(id: number) {
    return this.http.get<User>(this.baseUrl + "/users/" + id);
  }

  updateUser(user: User) {
    return this.http.put(this.baseUrl + "/users", user);
  }
}
