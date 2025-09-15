// ReSharper disable NotAccessedPositionalProperty.Global
using API.Helpers;

namespace API.DTO.Responses;

public record PaginatedResponseDto<T>(Page? Previous, Page Current, Page? Next, int TotalCount, int TotalPages, IEnumerable<T> Items) {
  public static PaginatedResponseDto<T> FromPaginationInfo(PaginationInfo paginationInfo, IEnumerable<T> items) 
    => new(paginationInfo.Previous?.Page, paginationInfo.Page, paginationInfo.Next?.Page, paginationInfo.TotalCount, paginationInfo.TotalPages, items);
  public static PaginatedResponseDto<T> Empty(int pageSize) => new(null, new Page(1, pageSize), null, 0, 0, []);
}
