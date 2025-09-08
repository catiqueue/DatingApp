using API.Data.Requests;
using API.Data.Responses;
using API.Entities;
using API.Extensions;
using API.Services.Abstractions;

using AutoMapper;

using Microsoft.AspNetCore.SignalR;

using static API.SignalR.Utils;

namespace API.SignalR;

public sealed class MessageHub(IUnitOfWork work, IMapper mapper, IHubContext<PresenceHub> presence, IPresenceTracker presenceTracker) : Hub {
  
  public override async Task OnConnectedAsync() {
    var httpContext = Context.GetHttpContext();
    if(httpContext?.Request.Query["user"].ToString() is not { } recipient || Context.User?.GetUsername() is not { } sender)
      throw new HubException("Cannot create a chat: one or both participants are missing.");
    var groupName = GetGroupName(sender, recipient);
    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    if (await AddToGroup(groupName, sender, Context.ConnectionId) is { } group)
      await Clients.Group(groupName).SendAsync("OnGroupUpdated", group);
    await Clients.Caller.SendAsync("OnMessageThreadReceived", await work.Messages.GetMessageThread(sender, recipient));
  }
  
  public async Task OnMessageSent(CreateMessageRequest request) {
    if(Context.User?.GetUsername() is not { } sender 
        || string.IsNullOrWhiteSpace(request.RecipientUsername)
        || string.Equals(sender, request.RecipientUsername, StringComparison.InvariantCultureIgnoreCase)) 
      throw new HubException("Cannot send a message: one or both participants are missing.");
    var groupName = GetGroupName(sender, request.RecipientUsername);
    
    if(await work.Users.GetDbUserAsync(request.RecipientUsername) is not { UserName: not null } recipientUser 
        || await work.Users.GetDbUserAsync(sender) is not { UserName: not null } senderUser)
      throw new HubException("Cannot send a message: one or both participants do not exist.");

    var message = new DbMessage {
      Sender = senderUser,
      SenderUsername = senderUser.UserName,

      Recipient = recipientUser,
      RecipientUsername = recipientUser.UserName,

      Content = request.Content
    };
    
    if(await work.Messages.GetGroup(groupName) is { } group 
        && group.Connections.Any(connection => connection.Username == request.RecipientUsername)) {
      message.ReadAt = DateTime.UtcNow;
    } else if(await presenceTracker.IsOnline(request.RecipientUsername)) {
      await presence.Clients.Clients(await presenceTracker.GetUserConnectionsAsync(request.RecipientUsername))
        .SendAsync("OnNewMessageReceived", new { username = senderUser.UserName, knownAs = senderUser.KnownAs });
    }
    
    work.Messages.AddMessage(message);
    
    if(await work.TrySaveAllAsync())
      await Clients.Group(groupName).SendAsync("OnMessageReceived", mapper.Map<SimpleMessage>(message));
  }

  private async Task<DbGroup?> AddToGroup(string groupName, string username, string connectionId) {
    if (await work.Messages.GetGroup(groupName) is not { } group) {
      group = new DbGroup { Name = groupName };
      work.Messages.AddGroup(group);
    }
    group.Connections.Add(new DbGroupConnection { ConnectionId = connectionId, Username = username });
    return await work.TrySaveAllAsync() ? group : throw new HubException("Failed to add to a group.");
  }
  
  private async Task<DbGroup?> RemoveFromGroup(string connectionId) {
    if (await work.Messages.GetConnectionGroup(connectionId) is not { } group ||
        group.Connections.FirstOrDefault(c => c.ConnectionId == connectionId) is not { } connection) 
      return null;
    
    work.Messages.RemoveConnection(connection);
    return await work.TrySaveAllAsync() ? group : throw new HubException("Failed to remove from a group.");
  }

  public override async Task OnDisconnectedAsync(Exception? exception) {
    if (await RemoveFromGroup(Context.ConnectionId) is { } group)
      await Clients.Group(group.Name).SendAsync("OnGroupUpdated", group);
    await base.OnDisconnectedAsync(exception);
  }
}

file static class Utils {
  public static string GetGroupName(string first, string second) =>
    string.CompareOrdinal(first, second) < 0 ? $"{first}-{second}" : $"{second}-{first}";
}
