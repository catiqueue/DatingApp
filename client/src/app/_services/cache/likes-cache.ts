import { Injectable, signal, WritableSignal } from "@angular/core";
import { AbstractCache, CacheWithGetters } from "./abstract-cache";
import { LikedListType } from "../../_models/likes-request";
import { User } from "../../_models/user";
import { PaginationInfo } from "../../_models/pagination";

export class LikesCacheSchema {
  likedIds = signal<number[]>([]);
  users = signal<User[]>([]);
  pagination = signal<PaginationInfo | undefined>(undefined);
  predicate = signal<LikedListType>(LikedListType.Liked);
}

@Injectable({ providedIn: 'root' })
export class LikesCacheService extends AbstractCache<LikesCacheSchema> implements CacheWithGetters<LikesCacheSchema> {
  public constructor() { super(LikesCacheSchema); }
  public get likedIds(){ return this.get("likedIds"); }
  public get predicate() { return this.get("predicate"); }
  public get users() { return this.get("users"); }
  public get pagination() { return this.get("pagination"); }

  clearFilters() {
    this.reset("predicate");
    this.reset("pagination");
  }
}
