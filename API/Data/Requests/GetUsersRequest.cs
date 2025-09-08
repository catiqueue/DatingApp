using API.Entities;
using API.Helpers;

namespace API.Data.Requests;

public sealed record GetUsersRequest(
                 UserGender? Gender = null,
                 int? MinAge = null,
                 int? MaxAge = null,
                 UserSortOrder? OrderBy = null) : PaginatedRequestBase
{
  public UserFilter ToFilter(string initiatorUsername) => new(Gender, MinAge, MaxAge, [initiatorUsername]);
}
