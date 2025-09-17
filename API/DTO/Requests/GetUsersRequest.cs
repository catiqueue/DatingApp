using API.Entities;
using API.Helpers;

namespace API.DTO.Requests;

public sealed class GetUsersRequest : PaginatedRequestBase {
  public UserSortOrder? OrderBy { get; set; }
  public Gender? Gender { get; set; }
  public int? MinAge { get; set; }
  public int? MaxAge { get; set; }
  
  public UserFilter ToFilter(string initiatorUsername) => new(Gender, MinAge, MaxAge, [initiatorUsername]);
}
