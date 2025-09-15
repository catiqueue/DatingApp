namespace API.Interfaces;

public enum AddedConnectionStatus { FirstConnection, AlreadyConnected }
public enum RemovedConnectionStatus { LastConnection, SecondConnection, NoneRemoved }

public interface IPresenceTrackerService {
  Task<bool> IsOnline(string username);
  Task<IEnumerable<string>> GetOnlineUsersAsync();
  Task<AddedConnectionStatus> AddConnectionAsync(string username, string connectionId);
  Task<RemovedConnectionStatus> RemoveConnectionAsync(string username, string connectionId);
  Task<IEnumerable<string>> GetUserConnectionsAsync(string username);
}
