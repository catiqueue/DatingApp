using System.ComponentModel.DataAnnotations;

using API.Helpers;

namespace API.Data.Requests;

public abstract record PaginatedRequestBase(int PageNumber = 1, [Range(5, 30)] int PageSize = 10) {
  public Page ToPage() => new(PageNumber, PageSize);
}
