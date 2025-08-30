export type Page = {
  pageNumber: number;
  pageSize: number;
}

export type PaginationInfo = {
  previous?: Page;
  current: Page;
  next?: Page;
  totalCount: number;
  totalPages: number;
}

export type PaginatedResponse<T> = {
  previous?: Page;
  current: Page;
  next?: Page;
  totalCount: number;
  totalPages: number;
  items: T[];
}
