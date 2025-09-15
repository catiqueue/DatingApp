import { inject, Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import { User } from "../_models/user";
import {Page} from '../_models/pagination';
import {appendHttpParams} from '../_utils/http-utils';
import {readPaginatedResponse} from '../_utils/pagination-utils';
import {AdminCacheService} from './cache/admin-cache';


@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  private adminCache = inject(AdminCacheService);

  public get photos() { return this.adminCache.photos; }
  public get pagination() { return this.adminCache.pagination; }

  getUsersWithRoles() {
    return this.http.get<User[]>(this.baseUrl + "/admin/users-with-roles/");
  }

  updateUserRoles(username: string, roles: string[]) {
    return this.http.post<string[]>(this.baseUrl + "/admin/edit-roles/" + username, roles);
  }

  loadPhotosForModeration() {
    this.loadPhotosForModerationInternal(this.pagination()?.current);
  }

  private loadPhotosForModerationInternal(page?: Page) {
    let queryParams = new HttpParams();
    if(page) queryParams = appendHttpParams(queryParams, page);
    readPaginatedResponse(this.http, this.baseUrl + "/admin/photos-to-moderate", queryParams, this.photos, this.pagination);
  }

  approvePhoto(photoId: number) {
    return this.http.post(this.baseUrl + "/admin/approve-photo/" + photoId, {});
  }

  rejectPhoto(photoId: number) {
    return this.http.post(this.baseUrl + "/admin/reject-photo/" + photoId, {});
  }


}
