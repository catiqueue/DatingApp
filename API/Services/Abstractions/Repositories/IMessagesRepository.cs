using API.Data.Responses;
using API.Entities;
using API.Helpers;

namespace API.Services.Abstractions.Repositories;

public enum MessageBoxFilter { Inbox, Outbox, Unread }

public interface IMessagesRepository {
  Task<bool> TrySaveAllAsync();
  Task<int> CountAsync(MessageBoxFilter filter, string recipient);
  void AddMessage(DbMessage message);
  void DeleteMessage(DbMessage message);
  Task<DbMessage?> GetMessage(uint id);
  Task<IEnumerable<SimpleMessage>> GetUserMessages(Page page, MessageBoxFilter filter, string username);
  Task<IEnumerable<SimpleMessage>> GetMessageThread(string caller, string other);
}
