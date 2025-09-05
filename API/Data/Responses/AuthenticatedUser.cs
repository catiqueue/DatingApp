using API.Entities;

namespace API.Data.Responses;

public sealed record AuthenticatedUser(
  string UserName,
  string KnownAs,
  UserGender Gender,
  string Token,
  string? AvatarUrl) 
{
  public static AuthenticatedUser FromDbUser(DbUser user, string token)
    => new(user.UserName!, user.KnownAs, user.Gender, token, user.Photos.FirstOrDefault(x => x.IsMain)?.Url);
}
