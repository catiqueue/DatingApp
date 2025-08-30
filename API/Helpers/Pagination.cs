using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace API.Helpers;

public sealed record Page(int PageNumber, int PageSize);

public sealed record PaginationInfo {
  public required Page Page { get; init; }
  public required int TotalCount { get; init; }
  public int TotalPages => TotalCount / Page.PageSize + (TotalCount % Page.PageSize == 0 ? 0 : 1);
  
  [JsonIgnore]
  public PaginationInfo? Next => TryCreate(Page with { PageNumber = Page.PageNumber + 1 }, TotalCount, out var result) ? result : null;
  [JsonIgnore]
  public PaginationInfo? Previous => TryCreate(Page with { PageNumber = Page.PageNumber - 1 }, TotalCount, out var result) ? result : null;
  
  public static bool TryCreate(Page page, int totalCount, [NotNullWhen(true)] out PaginationInfo? result) {
    result = new PaginationInfo { Page = page, TotalCount = totalCount };
    return page.PageNumber > 0 && page.PageNumber <= result.TotalPages;
  }
}

public static class PaginationExtensions {
  public static IQueryable<T> Slice<T>(this IQueryable<T> query, Page page) => query.Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize);
}
