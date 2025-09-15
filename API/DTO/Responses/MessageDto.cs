namespace API.DTO.Responses;

public class MessageDto {
  public int Id { get; set; }
  
  public int SenderId { get; set; }
  public required string SenderUsername { get; set; }
  public required string SenderAvatarUrl { get; set; }
  
  public int RecipientId { get; set; }
  public required string RecipientUsername { get; set; }
  public required string RecipientAvatarUrl { get; set; }
  
  public required string Content { get; set; }
  
  public DateTime? ReadAt { get; set; }
  public DateTime SentAt { get; set; }
}
