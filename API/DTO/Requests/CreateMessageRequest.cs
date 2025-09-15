using System.ComponentModel.DataAnnotations;

namespace API.DTO.Requests;

public sealed class CreateMessageRequest {
  public required string RecipientUsername { get; set; }
  public required string Content { get; set; }
}
