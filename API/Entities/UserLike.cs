namespace API.Entities;

public sealed class UserLike {
  public User LikingUser { get; set; } = null!;
  public int LikingUserId { get; set; }
  public User LikedUser { get; set; } = null!;
  public int LikedUserId { get; set; }
}
