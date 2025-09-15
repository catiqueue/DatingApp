using System.ComponentModel.DataAnnotations;

using API.Helpers;

namespace API.DTO.Requests;

public abstract class PaginatedRequestBase() {
  protected PaginatedRequestBase(int pageNumber, int pageSize) : this() {
    PageNumber = pageNumber;
    PageSize = pageSize;
  }
  
  public int PageNumber { get; set; } = 1;
  [Range(5, 30)] public int PageSize { get; set; } = 10;
  
  public Page ToPage() => new(PageNumber, PageSize);
}
