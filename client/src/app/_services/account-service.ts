import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { LoggedInUser } from '../_models/logged-in-user';
import { LoginForm } from '../_models/login-form';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { Photo } from '../_models/photo';
import { UsersCacheService } from './cache/users-cache';
import { LikesCacheService } from './cache/likes-cache';
import { LikesService } from './likes-service';
import { MessagesCacheService } from './cache/messages-cache';
import { PresenceService } from './presence-service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  // TODO: make a global cache service to access specific caches from (so i can clear them with one call)
  private usersCache = inject(UsersCacheService);
  private likesCache = inject(LikesCacheService);
  private messagesCache = inject(MessagesCacheService);

  private likesService = inject(LikesService);
  private presenceService = inject(PresenceService);

  private baseUrl = environment.apiUrl;

  public currentUser = signal<LoggedInUser | undefined>(undefined);
  public currentRoles = computed(() => {
    var user = this.currentUser();
    if(!user) return [];
    var roles = JSON.parse(atob(user.token.split(".")[1])).role;
    return Array.isArray(roles) ? roles : [roles];
  });

  public login(model: LoginForm) {
    return this.http.post<LoggedInUser>(this.baseUrl + "/account/login", model).pipe(map(
      user => {
        if(user) this.setCurrentUser(user)
        return user;
      }
    ));
  }

  public register(model: any) {
    return this.http.post<LoggedInUser>(this.baseUrl + "/account/register", model).pipe(map(
      user => {
        if(user) this.setCurrentUser(user)
        return user;
     }
    ));
  }

  private postAuthenticationAction() {
    this.presenceService.createHubConnection(this.currentUser()!);
    this.likesService.loadLikedIds();
  }

  public loadCurrentUser() {
    var userString = localStorage.getItem("user");
    if(!userString) return;
    var user: LoggedInUser = JSON.parse(userString);
    this.currentUser.set(user);
    this.postAuthenticationAction();
  }

  private setCurrentUser(user: LoggedInUser) {
    localStorage.setItem("user", JSON.stringify(user));
    this.currentUser.set(user);
    this.postAuthenticationAction();
  }

  public setAvatar(photo: Photo) {
    if(!this.currentUser()) return;
    var updated = {...this.currentUser()!};
    updated.avatarUrl = photo.url;
    this.setCurrentUser(updated);
  }

  public unsetCurrentUser() {
    localStorage.removeItem("user");
    this.currentUser.set(undefined);
    this.usersCache.clearAll();
    this.likesCache.clearAll();
    this.messagesCache.clearAll();
    this.presenceService.closeHubConnection();
  }
}
