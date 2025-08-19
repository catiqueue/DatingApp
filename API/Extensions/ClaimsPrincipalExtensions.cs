using System.Security.Claims;

using Microsoft.IdentityModel.JsonWebTokens;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions {
  public static string GetUsername(this ClaimsPrincipal user) 
    => user.FindFirstValue(JwtRegisteredClaimNames.Nickname) ?? throw new InvalidOperationException("Can't get the username from the token of an authorized user. How did you do that?");
}
