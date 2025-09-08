import { HttpClient, HttpParams, HttpRequest } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';
import { Photo } from '../_models/photo';
import { Page } from '../_models/pagination';
import { FilterParams } from '../_models/filer-params';
import { SortOrder } from '../_models/sort-order';
import { UsersCacheSchema, UsersCacheService } from './cache/users-cache';
import { readPaginatedResponse } from '../_utils/pagination-utils';
import { CacheWithGetters } from './cache/abstract-cache';
import { appendHttpParams } from '../_utils/http-utils';

@Injectable({
  providedIn: 'root'
})
export class UsersService implements CacheWithGetters<UsersCacheSchema> {
  private http = inject(HttpClient);
  private cache = inject(UsersCacheService);
  private baseUrl = environment.apiUrl;

  public get users() { return this.cache.users; }
  public get pagination() { return this.cache.pagination; }
  public get filter() { return this.cache.filter; }
  public get sortOrder() { return this.cache.sortOrder; }
  public clearFilters() { this.cache.clearFilters(); }

  public loadUsers() {
    this.loadUsersInternal(this.pagination()?.current, this.filter(), this.sortOrder());
  }

  private loadUsersInternal(page?: Page, filter?: FilterParams, order?: SortOrder) {
    var queryParams = new HttpParams();
    if(page) queryParams = appendHttpParams(queryParams, page);
    if(filter) queryParams = appendHttpParams(queryParams, filter);
    if(order) queryParams = queryParams.append("orderBy", order);
    readPaginatedResponse(this.http, this.baseUrl + "/users", queryParams, this.users, this.pagination);
  }

  public getUserByUsername(username: string) {
    return this.http.get<User>(this.baseUrl + "/users/" + username);
  }

  public getUserById(id: number) {
    return this.http.get<User>(this.baseUrl + "/users/" + id);
  }

  public updateUser(user: User) {
    return this.http.put(this.baseUrl + "/users", user);
  }

  public setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + "/users/set-main-photo/" + photo.id, {});
  }

  public deletePhoto(photo: Photo) {
    return this.http.delete(this.baseUrl + "/users/delete-photo/" + photo.id);
  }
}
