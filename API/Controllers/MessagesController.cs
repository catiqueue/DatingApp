using API.Data.Requests;
using API.Data.Responses;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Services.Abstractions;
using API.Services.Abstractions.Repositories;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class MessagesController(IMessagesRepository messages, IUserRepository users, IMapper mapper) : ApiControllerBase {
  [HttpPost]
  public async Task<ActionResult<SimpleMessage>> CreateMessage(CreateMessageRequest request) {
    var senderUsername = User.GetUsername();
    if(senderUsername == request.RecipientUsername.ToLower()) 
      return BadRequest("You can't send a message to yourself.");

    if(await users.GetDbUserAsync(senderUsername) is not {} sender) 
      return BadRequest("Could not find you in the database. How did you do that?");
    if(await users.GetDbUserAsync(request.RecipientUsername) is not {} recipient) 
      return BadRequest("Could not find the recipient in the database.");

    var message = new DbMessage {
      Sender = sender,
      SenderUsername = sender.Username,
      
      Recipient = recipient,
      RecipientUsername = recipient.Username,
      
      Content = request.Content
    };

    messages.AddMessage(message);
    return await messages.TrySaveAllAsync()
      ? Ok(mapper.Map<SimpleMessage>(message))
      // ? CreatedAtAction(nameof(GetMessage), new { id = message.Id }, mapper.Map<SimpleMessage>(message))
      : BadRequest("Failed to save the message.");
  }

  [HttpGet]
  public async Task<ActionResult<PaginatedResponse<SimpleMessage>>> GetMessages([FromQuery] GetMessagesRequest request)
    => PaginationInfo.TryCreate(request.ToPage(), await messages.CountAsync(request.Box, User.GetUsername()), out var paginationInfo) 
      ? Ok(PaginatedResponse<SimpleMessage>.FromPaginationInfo(
          paginationInfo,
          await messages.GetUserMessages(request.ToPage(), request.Box, User.GetUsername()))) 
      : request.PageNumber == 1
        ? Ok(PaginatedResponse<SimpleMessage>.Empty(request.PageSize))
        : BadRequest($"Page {request.PageNumber} does not exist.");
  
  [HttpGet("thread/{recipient}")]
  public async Task<ActionResult<IEnumerable<SimpleMessage>>> GetMessageThread(string recipient) 
    => Ok(await messages.GetMessageThread(User.GetUsername(), recipient));

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteMessage(uint id) {
    var username = User.GetUsername();
    
    if(await messages.GetMessage(id) is not { } message)
      return BadRequest("This message does not exist.");

    if(message.SenderUsername != username && message.RecipientUsername != username)
      return Forbid();

    if (username == message.SenderUsername) message.SenderDeleted = true;
    if (username == message.RecipientUsername) message.RecipientDeleted = true;
    
    if(message is { SenderDeleted: true, RecipientDeleted: true }) 
      messages.DeleteMessage(message);
    
    return await messages.TrySaveAllAsync() ? Ok() : BadRequest("Failed to delete the message.");
  }
}
