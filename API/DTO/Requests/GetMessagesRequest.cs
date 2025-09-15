using System.ComponentModel.DataAnnotations;

using API.Interfaces.Repositories;

namespace API.DTO.Requests;

public sealed class GetMessagesRequest : PaginatedRequestBase {
  public MessageBoxFilter Box { get; set; } = MessageBoxFilter.Unread;
}
