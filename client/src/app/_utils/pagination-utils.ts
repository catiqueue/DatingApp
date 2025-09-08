import { HttpClient, HttpParams } from "@angular/common/http";
import { WritableSignal } from "@angular/core";
import { PaginatedResponse, PaginationInfo } from "../_models/pagination";

export function readPaginatedResponse<T>(client: HttpClient, url: string, params: HttpParams, data: WritableSignal<T[]>, pagination: WritableSignal<PaginationInfo | undefined>) {
  client.get<PaginatedResponse<T>>(url, {params: params}).subscribe({
    next: response => {
      data.set(response.items);
      pagination.set(response);
    },
    error: (_) => { data.set([]); pagination.set(undefined); }
  });
}

export function setPageToOne(pagination?: PaginationInfo) : PaginationInfo | undefined {
  return pagination ? {...pagination, current: {...pagination?.current, pageNumber: 1}} : undefined;
}
