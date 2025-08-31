import { HttpClient, HttpParams, HttpRequest } from "@angular/common/http";
import { WritableSignal } from "@angular/core";
import { Page, PaginatedResponse, PaginationInfo } from "../_models/pagination";

export function appendHttpParams<T extends Record<string, any>>(params: HttpParams, object: T) {
  (Object.keys(object)).forEach((key) => {
    var value = object[key];
    if (value !== undefined && value !== null) {
      params = params.append(String(key), String(value));
    }
  });
  return params;
}

export function readPaginatedResponse<T>(client: HttpClient, url: string, params: HttpParams, data: WritableSignal<T[]>, pagination: WritableSignal<PaginationInfo>) {
  client.get<PaginatedResponse<T>>(url, {params: params}).subscribe({
    next: response => {
      data.set(response.items);
      pagination.set(response);
    }
  });
}
