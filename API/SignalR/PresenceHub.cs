using API.Extensions;
using API.Services.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public sealed class PresenceHub(IPresenceTracker tracker) : Hub {
  public override async Task OnConnectedAsync() {
    if(await tracker.AddConnectionAsync(Context.User!.GetUsername(), Context.ConnectionId) is AddedConnectionStatus.FirstConnection)
      await Clients.Others.SendAsync("OnSomeoneConnected", Context.User!.GetUsername());
    
    var currentUsers = await tracker.GetOnlineUsersAsync();
    await Clients.Caller.SendAsync("OnOnlineUsersUpdated", currentUsers);
  }

  public override async Task OnDisconnectedAsync(Exception? exception) {
    if(await tracker.RemoveConnectionAsync(Context.User!.GetUsername(), Context.ConnectionId) is RemovedConnectionStatus.LastConnection)
      await Clients.Others.SendAsync("OnSomeoneDisconnected", Context.User?.GetUsername());
    await base.OnDisconnectedAsync(exception);
  }
}
