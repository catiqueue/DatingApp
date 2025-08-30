using System.ComponentModel.DataAnnotations;

using API.Entities;
using API.Helpers;

namespace API.Data.Requests;

public sealed record GetUsersRequest(
                 int Page = 1,
  [Range(5, 30)] int PageSize = 10,
                 UserGender? Gender = null,
                 int? MinAge = null,
                 int? MaxAge = null,
                 UserSortOrder? OrderBy = null) 
{
  public Page ToPage() => new(Page, PageSize);
  public UserFilter ToFilter(string initiatorUsername) => new(Gender, MinAge, MaxAge, [initiatorUsername]);
}
