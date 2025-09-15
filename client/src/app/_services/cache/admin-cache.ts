import { Injectable, signal } from "@angular/core";
import { AbstractCache, CacheWithGetters } from "./abstract-cache";
import { PaginationInfo } from "../../_models/pagination";
import { Photo } from "../../_models/photo";

export class AdminCacheSchema {
  photos = signal<Photo[]>([]);
  pagination = signal<PaginationInfo | undefined>(undefined);
}

@Injectable({ providedIn: 'root' })
export class AdminCacheService extends AbstractCache<AdminCacheSchema> implements CacheWithGetters<AdminCacheSchema> {
  public constructor() { super(AdminCacheSchema); }
  public get photos() { return this.get("photos"); }
  public get pagination() { return this.get("pagination"); }

  clearFilters() {
    this.reset("pagination");
  }
}
