import { Injectable, signal, WritableSignal } from "@angular/core";
import { FilterParams } from "../../_models/filer-params";
import { PaginationInfo } from "../../_models/pagination";
import { SortOrder } from "../../_models/sort-order";
import { User } from "../../_models/user";
import { AbstractCache, CacheWithGetters } from "./abstract-cache";

export class UsersCacheSchema {
  users = signal<User[]>([]);
  pagination = signal<PaginationInfo | undefined>(undefined);
  filter = signal<FilterParams>({});
  sortOrder = signal<SortOrder | undefined>(undefined);
}

@Injectable({ providedIn: 'root' })
export class UsersCacheService extends AbstractCache<UsersCacheSchema> implements CacheWithGetters<UsersCacheSchema> {
  public constructor() { super(UsersCacheSchema); }
  public get users() { return this.get("users"); }
  public get pagination() { return this.get("pagination"); }
  public get filter() { return this.get("filter"); }
  public get sortOrder() { return this.get("sortOrder"); }

  clearFilters() {
    this.reset("pagination");
    this.reset("filter");
    this.reset("sortOrder");
  }
}
