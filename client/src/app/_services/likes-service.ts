import { inject, Injectable, WritableSignal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LikesCacheService } from './cache/likes-cache';
import { LikedListType } from '../_models/likes-request';
import { Page, PaginationInfo } from '../_models/pagination';
import { appendHttpParams, readPaginatedResponse } from './pagination-utils';

@Injectable({
  providedIn: 'root'
})
export class LikesService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  private cache = inject(LikesCacheService);

  resetCache() {
    this.cache.clearFilters();
    this.loadLikedList();
  }

  toggleLike(liked: number) {
    return this.http.post(this.baseUrl + "/likes/" + liked, {});
  }

  loadLikedList() {
    this.loadLikedListInternal(this.cache.predicate(), this.cache.pagination()?.current);
  }

  private loadLikedListInternal(predicate: LikedListType, page?: Page) {
    var params = new HttpParams();
    if(page) params = appendHttpParams(params, page);
    params = params.append("predicate", predicate);
    readPaginatedResponse(this.http, this.baseUrl + "/likes", params, this.cache.users, this.cache.pagination);
  }

  loadLikedIds() {
    return this.http.get<number[]>(this.baseUrl + "/likes/list").subscribe({
      next: (response) => this.cache.likedIds.set(response)
    });
  }
}
