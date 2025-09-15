using API.Entities;
using API.Helpers;

namespace API.DTO.Requests;

public sealed class GetUsersRequest : PaginatedRequestBase {
  public UserSortOrder? OrderBy { get; set; }
  private Gender? Gender { get; set; }
  private int? MinAge { get; set; }
  private int? MaxAge { get; set; }
  
  public UserFilter ToFilter(string initiatorUsername) => new(Gender, MinAge, MaxAge, [initiatorUsername]);
}
