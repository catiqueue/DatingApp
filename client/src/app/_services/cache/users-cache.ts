import { Injectable, signal, WritableSignal } from "@angular/core";
import { FilterParams } from "../../_models/filer-params";
import { PaginationInfo } from "../../_models/pagination";
import { SortOrder } from "../../_models/sort-order";
import { User } from "../../_models/user";
import { AbstractCache } from "./abstract-cache";

export interface UsersCacheSchema {
  users: User[];
  pagination: PaginationInfo | undefined;
  filter: FilterParams;
  sortOrder: SortOrder | undefined;
}

@Injectable({ providedIn: 'root' })
export class UsersCacheService extends AbstractCache<UsersCacheSchema> {
  public get users(): WritableSignal<User[]> {
    return this.getOrCreate("users", () => []);
  }
  public get pagination(): WritableSignal<PaginationInfo | undefined> {
    return this.getOrCreate("pagination", () => undefined);
  }
  public get filter(): WritableSignal<FilterParams> {
    return this.getOrCreate("filter", () => ({ } as FilterParams));
  }
  public get sortOrder(): WritableSignal<SortOrder | undefined> {
    return this.getOrCreate("sortOrder", () => undefined);
  }

  clearFilters() {
    this.pagination.set(undefined);
    this.filter.set({});
    this.sortOrder.set(undefined);
  }

  clearAll() {
    this.clearFilters()
    this.users.set([]);
  }
}
