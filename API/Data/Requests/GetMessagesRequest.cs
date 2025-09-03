using System.ComponentModel.DataAnnotations;

using API.Helpers;
using API.Services.Abstractions.Repositories;

namespace API.Data.Requests;

/* public sealed class GetMessagesRequest {
  public required MessageBoxFilter Box { get; set; }
  public int PageNumber { get; set; } = 1;
  [Range(10, 30)] public int PageSize { get; set; } = 10;
  public Page ToPage() => new(PageNumber, PageSize);
} */

public sealed record GetMessagesRequest(MessageBoxFilter Box = MessageBoxFilter.Unread, int PageNumber = 1, [Range(10, 30)] int PageSize = 10) : PaginatedRequestBase(PageNumber, PageSize);
