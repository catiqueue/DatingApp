import { Injectable, WritableSignal } from "@angular/core";
import { AbstractCache } from "./abstract-cache";
import { LikedListType } from "../../_models/likes-request";
import { User } from "../../_models/user";
import { PaginationInfo } from "../../_models/pagination";

export interface LikesCacheSchema {
  likedIds: number[];
  users: User[];
  pagination: PaginationInfo | undefined;
  predicate: LikedListType;
}

@Injectable({ providedIn: 'root' })
export class LikesCacheService extends AbstractCache<LikesCacheSchema> {
  public get likedIds(): WritableSignal<number[]> {
    return this.getOrCreate("likedIds", () => []);
  }
  public get predicate(): WritableSignal<LikedListType> {
    return this.getOrCreate("predicate", () => LikedListType.Mutual);
  }
  public get users(): WritableSignal<User[]> {
    return this.getOrCreate("users", () => []);
  }
  public get pagination(): WritableSignal<PaginationInfo | undefined> {
    return this.getOrCreate("pagination", () => undefined);
  }

  clearFilters() {
    this.predicate.set(LikedListType.Mutual);
    this.pagination.set(undefined);
  }

  clearAll() {
    this.clearFilters();
    this.likedIds.set([]);
    this.users.set([]);
  }
}
