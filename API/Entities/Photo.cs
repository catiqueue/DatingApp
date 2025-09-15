namespace API.Entities;

public sealed class Photo : EntityBase<Photo> {
  public bool IsMain { get; set; }
  public bool IsApproved { get; set; }
  
  public required string Url { get; set; }
  public string? PublicId { get; set; }
  
  public int UserId { get; set; }
  public User User { get; set; } = null!;
}
