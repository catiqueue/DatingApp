using System.ComponentModel.DataAnnotations;

using API.Repositories;

namespace API.DTO.Requests;

public sealed class GetLikedListRequest : PaginatedRequestBase {
  public required LikedListType Predicate { get; set; }
}
