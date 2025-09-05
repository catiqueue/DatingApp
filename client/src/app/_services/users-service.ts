import { HttpClient, HttpParams, HttpRequest } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';
import { Photo } from '../_models/photo';
import { Page, PaginationInfo } from '../_models/pagination';
import { FilterParams } from '../_models/filer-params';
import { SortOrder } from '../_models/sort-order';
import { UsersCacheService } from './cache/users-cache';
import { readPaginatedResponse, appendHttpParams } from './pagination-utils';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private http = inject(HttpClient);
  private cache = inject(UsersCacheService);
  baseUrl = environment.apiUrl;

  public loadUsers() {
    this.loadUsersInternal(this.cache.pagination()?.current, this.cache.filter(), this.cache.sortOrder());
  }

  private loadUsersInternal(page?: Page, filter?: FilterParams, order?: SortOrder) {
    var queryParams = new HttpParams();
    if(page) queryParams = appendHttpParams(queryParams, page);
    if(filter) queryParams = appendHttpParams(queryParams, filter);
    if(order) queryParams = queryParams.append("orderBy", order);
    readPaginatedResponse(this.http, this.baseUrl + "/users", queryParams, this.cache.users, this.cache.pagination);
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

  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + "/users/set-main-photo/" + photo.id, {});
  }

  deletePhoto(photo: Photo) {
    return this.http.delete(this.baseUrl + "/users/delete-photo/" + photo.id);
  }
}
