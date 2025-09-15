using System.Linq.Expressions;

using API.Data;
using API.DTO.Responses;
using API.Entities;
using API.Helpers;
using API.Interfaces.Repositories;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using static API.Repositories.MessageHelpers;

namespace API.Repositories;

public class MessageRepository(ApiDbContext db, IMapper mapper) : IMessageRepository {
  public async Task<int> CountAsync(MessageBoxFilter filter, string recipient) => await ReadOnlyMessages.Filter(filter, recipient).CountAsync();
  public void AddMessage(Message message) => db.Messages.Add(message);
  public void DeleteMessage(Message message) => db.Messages.Remove(message);
  public async Task<Message?> GetMessage(int id) => await db.Messages.FindAsync(id);
  public async Task<IEnumerable<MessageDto>> GetUserMessages(Page page, MessageBoxFilter filter, string username) 
    => await ReadOnlyMessages
      .OrderByDescending(message => message.SentAt)
      .Filter(filter, username)
      .Slice(page)
      .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
      .ToListAsync();

  // this method probably doesn't follow the unit of work pattern, 
  // because it executes the update on the database directly.
  // but I don't want to refactor this at the moment.
  public async Task<IEnumerable<MessageDto>> GetMessageThread(string caller, string other) {
    await ReadOnlyMessages
      .Where(MessageIsFrom(other))
      .Where(MessageIsTo(caller))
      .Where(MessageIsUnread)
      .ExecuteUpdateAsync(setter => setter.SetProperty(message => message.ReadAt, _ => DateTime.UtcNow));
    
    return await ReadOnlyMessages
      .Where(MessageIsFromThread(caller, other))
      .OrderBy(message => message.SentAt)
      .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
      .ToListAsync();
  }

  public void AddGroup(Group group) => db.Groups.Add(group);

  public void RemoveConnection(GroupConnection connection) => db.Connections.Remove(connection);

  public async Task<GroupConnection?> GetConnection(string connectionId) 
    => await db.Connections.FindAsync(connectionId);

  public async Task<Group?> GetGroup(string groupName) 
    => await db.Groups.Include(group => group.Connections)
                      .FirstOrDefaultAsync(group => group.Name == groupName);
  
  public async Task<Group?> GetConnectionGroup(string connectionId)
    => await db.Groups.Include(group => group.Connections)
                      .FirstOrDefaultAsync(group => group.Connections.Any(connection => connection.ConnectionId == connectionId));

  private IQueryable<Message> ReadOnlyMessages => db.Messages.AsNoTracking();
}

file static class MessageHelpers {
  public static Expression<Func<Message, bool>> MessageIsFrom(string username) 
    => message => message.SenderUsername == username && message.SenderDeleted == false;
  
  public static Expression<Func<Message, bool>> MessageIsTo(string username) 
    => message => message.RecipientUsername == username && message.RecipientDeleted == false;
  
  public static Expression<Func<Message, bool>> MessageIsFromThread(string caller, string other) 
    => message => (caller == message.SenderUsername && message.RecipientUsername == other && !message.SenderDeleted) 
               || (caller == message.RecipientUsername && message.SenderUsername == other && !message.RecipientDeleted);
  
  public static Expression<Func<Message, bool>> MessageIsUnread => message => message.ReadAt == null;
  
  public static IQueryable<Message> Filter(this IQueryable<Message> query, MessageBoxFilter filter, string subject) => filter switch {
    MessageBoxFilter.Outbox => query.Where(MessageIsFrom(subject)),
    MessageBoxFilter.Inbox => query.Where(MessageIsTo(subject)),
    MessageBoxFilter.Unread => query.Where(MessageIsTo(subject)).Where(MessageIsUnread),
    _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
  };
}
