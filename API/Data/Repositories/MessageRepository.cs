using System.Linq.Expressions;

using API.Data.Responses;
using API.Entities;
using API.Helpers;
using API.Services.Abstractions.Repositories;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using static API.Data.Repositories.MessageHelpers;

namespace API.Data.Repositories;

public class MessageRepository(DataContext db, IMapper mapper) : IMessagesRepository {
  public async Task<int> CountAsync(MessageBoxFilter filter, string recipient) => await ReadOnlyMessages.Filter(filter, recipient).CountAsync();
  public void AddMessage(DbMessage message) => db.Messages.Add(message);
  public void DeleteMessage(DbMessage message) => db.Messages.Remove(message);
  public async Task<DbMessage?> GetMessage(uint id) => await db.Messages.FindAsync(id);
  public async Task<IEnumerable<SimpleMessage>> GetUserMessages(Page page, MessageBoxFilter filter, string username) 
    => await ReadOnlyMessages
      .OrderByDescending(message => message.SentAt)
      .Filter(filter, username)
      .Slice(page)
      .ProjectTo<SimpleMessage>(mapper.ConfigurationProvider)
      .ToListAsync();

  // this method probably doesn't follow the unit of work pattern, 
  // because it executes the update on the database directly.
  // but I don't want to refactor this at the moment.
  public async Task<IEnumerable<SimpleMessage>> GetMessageThread(string caller, string other) {
    await ReadOnlyMessages
      .Where(MessageIsFrom(other))
      .Where(MessageIsTo(caller))
      .Where(MessageIsUnread)
      .ExecuteUpdateAsync(setter => setter.SetProperty(message => message.ReadAt, _ => DateTime.UtcNow));
    
    return await ReadOnlyMessages
      .Where(MessageIsFromThread(caller, other))
      .OrderBy(message => message.SentAt)
      .ProjectTo<SimpleMessage>(mapper.ConfigurationProvider)
      .ToListAsync();
  }

  public void AddGroup(DbGroup group) => db.Groups.Add(group);

  public void RemoveConnection(DbGroupConnection connection) => db.Connections.Remove(connection);

  public async Task<DbGroupConnection?> GetConnection(string connectionId) 
    => await db.Connections.FindAsync(connectionId);

  public async Task<DbGroup?> GetGroup(string groupName) 
    => await db.Groups.Include(group => group.Connections)
                      .FirstOrDefaultAsync(group => group.Name == groupName);
  
  public async Task<DbGroup?> GetConnectionGroup(string connectionId)
    => await db.Groups.Include(group => group.Connections)
                      .FirstOrDefaultAsync(group => group.Connections.Any(connection => connection.ConnectionId == connectionId));

  private IQueryable<DbMessage> ReadOnlyMessages => db.Messages.AsNoTracking();
}

file static class MessageHelpers {
  public static Expression<Func<DbMessage, bool>> MessageIsFrom(string username) 
    => message => message.SenderUsername == username && message.SenderDeleted == false;
  
  public static Expression<Func<DbMessage, bool>> MessageIsTo(string username) 
    => message => message.RecipientUsername == username && message.RecipientDeleted == false;
  
  public static Expression<Func<DbMessage, bool>> MessageIsFromThread(string caller, string other) 
    => message => (caller == message.SenderUsername && message.RecipientUsername == other && !message.SenderDeleted) 
               || (caller == message.RecipientUsername && message.SenderUsername == other && !message.RecipientDeleted);
  
  public static Expression<Func<DbMessage, bool>> MessageIsUnread => message => message.ReadAt == null;
  
  public static IQueryable<DbMessage> Filter(this IQueryable<DbMessage> query, MessageBoxFilter filter, string subject) => filter switch {
    MessageBoxFilter.Outbox => query.Where(MessageIsFrom(subject)),
    MessageBoxFilter.Inbox => query.Where(MessageIsTo(subject)),
    MessageBoxFilter.Unread => query.Where(MessageIsTo(subject)).Where(MessageIsUnread),
    _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
  };
}
