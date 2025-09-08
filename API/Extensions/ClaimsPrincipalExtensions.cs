using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions {
  public static string GetUsername(this ClaimsPrincipal user) 
    => user.FindFirstValue(ClaimTypes.Name) ?? throw new InvalidOperationException("Can't get the username from the token of an authorized user. How did you do that?");
  public static uint GetId(this ClaimsPrincipal user) 
    => user.FindFirstValue(ClaimTypes.NameIdentifier) is { } value && uint.TryParse(value, out var id) 
      ? id
      : throw new InvalidOperationException("Can't get the id from the token of an authorized user. How did you do that?"); 
}
