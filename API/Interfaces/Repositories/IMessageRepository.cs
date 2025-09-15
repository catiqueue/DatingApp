using API.DTO.Responses;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.Repositories;

public enum MessageBoxFilter { Inbox, Outbox, Unread }

public interface IMessageRepository : IRepository {
  Task<int> CountAsync(MessageBoxFilter filter, string recipient);
  void AddMessage(Message message);
  void DeleteMessage(Message message);
  Task<Message?> GetMessage(int id);
  Task<IEnumerable<MessageDto>> GetUserMessages(Page page, MessageBoxFilter filter, string username);
  Task<IEnumerable<MessageDto>> GetMessageThread(string caller, string other);
  void AddGroup(Group group);
  void RemoveConnection(GroupConnection connection);
  Task<GroupConnection?> GetConnection(string connectionId);
  Task<Group?> GetGroup(string groupName);
  Task<Group?> GetConnectionGroup(string connectionId);
}
