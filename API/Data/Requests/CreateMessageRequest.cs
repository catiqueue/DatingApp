using System.ComponentModel.DataAnnotations;

namespace API.Data.Requests;

public sealed record CreateMessageRequest([Required] string RecipientUsername, [Required] string Content);
