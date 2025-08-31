namespace API.Entities;

public sealed class DbUserLike {
  public DbUser LikingUser { get; set; } = null!;
  public uint LikingUserId { get; set; }
  public DbUser LikedUser { get; set; } = null!;
  public uint LikedUserId { get; set; }
}
