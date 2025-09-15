using API.DTO.Requests;
using API.DTO.Responses;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class MessagesController(IUnitOfWork work, IMapper mapper) : ApiControllerBase {
  [HttpPost]
  public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageRequest request) {
    var senderUsername = User.GetUsername().ToLower();
    if(senderUsername == request.RecipientUsername.ToLower()) 
      return BadRequest("You can't send a message to yourself.");

    if(await work.Users.GetUserAsync(senderUsername) is not { UserName: not null } sender ) 
      return BadRequest("Could not find you in the database. How did you do that?");
    if(await work.Users.GetUserAsync(request.RecipientUsername) is not { UserName: not null } recipient) 
      return BadRequest("Could not find the recipient in the database.");

    var message = new Message {
      Sender = sender,
      SenderUsername = sender.UserName,
      
      Recipient = recipient,
      RecipientUsername = recipient.UserName,
      
      Content = request.Content
    };

    work.Messages.AddMessage(message);
    return await work.TrySaveAllAsync()
      ? Ok(mapper.Map<MessageDto>(message))
      // ? CreatedAtAction(nameof(GetMessage), new { id = message.Id }, mapper.Map<SimpleMessage>(message))
      : BadRequest("Failed to save the message.");
  }

  [HttpGet]
  public async Task<ActionResult<PaginatedResponseDto<MessageDto>>> GetMessages([FromQuery] GetMessagesRequest request)
    => PaginationInfo.TryCreate(request.ToPage(), await work.Messages.CountAsync(request.Box, User.GetUsername()), out var paginationInfo) 
      ? Ok(PaginatedResponseDto<MessageDto>.FromPaginationInfo(
          paginationInfo,
          await work.Messages.GetUserMessages(request.ToPage(), request.Box, User.GetUsername()))) 
      : request.PageNumber == 1
        ? Ok(PaginatedResponseDto<MessageDto>.Empty(request.PageSize))
        : BadRequest($"Page {request.PageNumber} does not exist.");
  
  [HttpGet("thread/{recipient}")]
  public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string recipient) 
    => Ok(await work.Messages.GetMessageThread(User.GetUsername(), recipient));

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteMessage(int id) {
    var username = User.GetUsername();
    
    if(await work.Messages.GetMessage(id) is not { } message)
      return BadRequest("This message does not exist.");

    if(message.SenderUsername != username && message.RecipientUsername != username)
      return Forbid();

    if (username == message.SenderUsername) message.SenderDeleted = true;
    if (username == message.RecipientUsername) message.RecipientDeleted = true;
    
    if(message is { SenderDeleted: true, RecipientDeleted: true }) 
      work.Messages.DeleteMessage(message);
    
    return await work.TrySaveAllAsync() ? Ok() : BadRequest("Failed to delete the message.");
  }
}
