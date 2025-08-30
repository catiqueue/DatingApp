import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';
import { Photo } from '../_models/photo';
import { Page, PaginatedResponse, PaginationInfo } from '../_models/pagination';
import { FilterParams } from '../_models/filer-params';
import { SortOrder } from '../_models/sort-order';
import { UsersCacheService } from './cache/users-cache';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private http = inject(HttpClient);
  private cache = inject(UsersCacheService);
  baseUrl = environment.apiUrl;
  /* users = signal<User[]>([]);
  pagination = signal<PaginationInfo | null>(null);
  filter = signal<FilterParams>({});
  sortOrder = signal<SortOrder | null>(null); */

  resetCache() {
    this.cache.clearFilters();
    this.loadUsers();
  }

  public loadUsers() {
    this.loadUsersInternal(this.cache.pagination()?.current, this.cache.filter(), this.cache.sortOrder());
  }

  private loadUsersInternal(page?: Page, filter?: FilterParams, order?: SortOrder) {
    var queryParams = new HttpParams();
    if(page) queryParams = this.setPaginationParams(queryParams, page);
    if(filter) queryParams = this.setQueryFilterParams(queryParams, filter);
    if(order) queryParams = queryParams.append("orderBy", order);
    this.http.get<PaginatedResponse<User>>(this.baseUrl + "/users", { params: queryParams }).subscribe({
      next: response => {
        this.cache.users.set(response.items)
        this.cache.pagination.set(response)
      }
    });
  }

  private setPaginationParams(params: HttpParams, page: Page) {
    return params.append("page", page.pageNumber).append("pageSize", page.pageSize);
  }
  private setQueryFilterParams(params: HttpParams, filter: FilterParams) {
    if(filter.gender) params = params.append("gender", filter.gender);
    if(filter.minAge) params = params.append("minAge", filter.minAge);
    if(filter.maxAge) params = params.append("maxAge", filter.maxAge);
    return params;
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
