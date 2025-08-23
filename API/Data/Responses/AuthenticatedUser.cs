namespace API.Data.DTOs;

public sealed record AuthenticatedUser(string Username, string KnownAs, string Token, string? AvatarUrl);
