import { HttpParams } from "@angular/common/http";

export function appendHttpParams<T extends Record<string, any>>(params: HttpParams, object: T) {
  (Object.keys(object)).forEach((key) => {
    var value = object[key];
    if (value !== undefined && value !== null) {
      params = params.append(String(key), String(value));
    }
  });
  return params;
}
