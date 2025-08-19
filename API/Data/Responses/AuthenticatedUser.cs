namespace API.Data.DTOs;

public sealed record AuthenticatedUser(string Username, string Token, string? AvatarUrl);
