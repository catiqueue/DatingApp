using API.Helpers;

namespace API.Data.Responses;

public record PaginatedResponse<T>(Page? Previous, Page Current, Page? Next, int TotalCount, int TotalPages, IEnumerable<T> Items) {
  public static PaginatedResponse<T> FromPaginationInfo(PaginationInfo paginationInfo, IEnumerable<T> items) 
    => new(paginationInfo.Previous?.Page, paginationInfo.Page, paginationInfo.Next?.Page, paginationInfo.TotalCount, paginationInfo.TotalPages, items);
  public static PaginatedResponse<T> Empty(int pageSize) => new(null, new Page(1, pageSize), null, 0, 0, []);
}
