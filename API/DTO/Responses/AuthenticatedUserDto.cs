using API.Entities;

namespace API.DTO.Responses;

public sealed class AuthenticatedUserDto {
  public required string UserName { get; set; }
  public required string KnownAs { get; set; }
  public required Gender Gender { get; set; }
  public required string Token { get; set; }
  public string? AvatarUrl { get; set; }

  public static AuthenticatedUserDto FromDbUser(User user, string token)
    => new() {
      UserName = user.UserName!,
      KnownAs = user.KnownAs,
      Gender = user.Gender,
      Token = token,
      AvatarUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
    };
}
