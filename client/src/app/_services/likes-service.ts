import { inject, Injectable, WritableSignal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LikesCacheSchema, LikesCacheService } from './cache/likes-cache';
import { LikedListType } from '../_models/likes-request';
import { Page } from '../_models/pagination';
import { readPaginatedResponse } from '../_utils/pagination-utils';
import { CacheWithGetters } from './cache/abstract-cache';
import { appendHttpParams } from '../_utils/http-utils';

@Injectable({
  providedIn: 'root'
})
export class LikesService implements CacheWithGetters<LikesCacheSchema> {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  private cache = inject(LikesCacheService);

  public get likedIds(){ return this.cache.likedIds; }
  public get predicate() { return this.cache.predicate; }
  public get users() { return this.cache.users; }
  public get pagination() { return this.cache.pagination; }

  public resetCache() {
    this.cache.clearFilters();
    this.loadLikedList();
  }

  public toggleLike(liked: number) {
    return this.http.post(this.baseUrl + "/likes/" + liked, {});
  }

  public loadLikedList() {
    this.loadLikedListInternal(this.predicate(), this.pagination()?.current);
  }

  private loadLikedListInternal(predicate: LikedListType, page?: Page) {
    var params = new HttpParams();
    if(page) params = appendHttpParams(params, page);
    params = params.append("predicate", predicate);
    readPaginatedResponse(this.http, this.baseUrl + "/likes", params, this.users, this.pagination);
  }

  public loadLikedIds() {
    return this.http.get<number[]>(this.baseUrl + "/likes/list").subscribe({
      next: (response) => this.cache.likedIds.set(response)
    });
  }
}
