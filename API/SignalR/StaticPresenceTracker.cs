using System.Collections.Concurrent;
using System.Collections.Immutable;

using API.Services.Abstractions;

namespace API.SignalR;

// man
public sealed class StaticPresenceTracker : IPresenceTracker {
  private static readonly ConcurrentDictionary<string, ImmutableHashSet<string>> UserConnections = [];
  
  public Task<bool> IsOnline(string username) 
    => Task.FromResult(UserConnections.ContainsKey(username));
  
  public Task<IEnumerable<string>> GetOnlineUsersAsync() 
    => Task.FromResult(UserConnections.Keys.AsEnumerable());
  
  public Task<AddedConnectionStatus> AddConnectionAsync(string username, string connectionId) {
    bool previouslyContained;
    lock (UserConnections) {
      previouslyContained = UserConnections.ContainsKey(username);
      UserConnections.AddOrUpdate(username, addValue: ImmutableHashSet<string>.Empty.Add(connectionId),
        updateValueFactory: (_, oldValue) => oldValue.Add(connectionId));  
    }

    return Task.FromResult(previouslyContained ? AddedConnectionStatus.AlreadyConnected : AddedConnectionStatus.FirstConnection);
  }

  public Task<RemovedConnectionStatus> RemoveConnectionAsync(string username, string connectionId) {
    lock (UserConnections) {
      if (!UserConnections.TryGetValue(username, out var connections) || !connections.Contains(connectionId)) 
        return Task.FromResult(RemovedConnectionStatus.NoneRemoved);
      // ReSharper disable once InvertIf - for readability
      // "connections" object is immutable, so .Count is before the removal
      if (UserConnections.TryUpdate(username, connections.Remove(connectionId), comparisonValue: connections) && connections.Count == 1) {
        UserConnections.TryRemove(username, out _);
        return Task.FromResult(RemovedConnectionStatus.LastConnection);
      }
      
      return Task.FromResult(RemovedConnectionStatus.SecondConnection);
    }
  }

  public Task<IEnumerable<string>> GetUserConnectionsAsync(string username)
    => Task.FromResult(UserConnections.TryGetValue(username, out var connections) ? connections 
                                                                                  : Enumerable.Empty<string>());
}
