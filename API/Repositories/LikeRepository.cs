using System.Linq.Expressions;

using API.Data;
using API.DTO.Responses;
using API.Entities;
using API.Helpers;
using API.Interfaces.Repositories;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public enum LikedListType { Liked, LikedBy, Mutual }

public sealed class LikeRepository(ApiDbContext db, IMapper mapper) : ILikeRepository {
  public Task<int> CountAsync(LikedListType request, int userId) => ReadOnlyLikes.FilterUsers(request, userId).CountAsync();

  public void AddLike(UserLike like) => db.Likes.Add(like);

  public void DeleteLike(UserLike like) => db.Likes.Remove(like);

  public async Task<UserLike?> GetLikeAsync(int likingUserId, int likedUserId)
    => await db.Likes.FindAsync(likingUserId, likedUserId);

  public async Task<IEnumerable<UserDto>> GetUserDtoLikedListAsync(Page page, LikedListType request, int userId) 
    => await ReadOnlyLikes
      .FilterUsers(request, userId)
      .Slice(page)
      .ProjectTo<UserDto>(mapper.ConfigurationProvider)
      .ToListAsync();

  public async Task<IEnumerable<int>> GetUsersLikedIdsAsync(int currentUserId)
    => await ReadOnlyLikes
      .Where(LikingUserIs(currentUserId))
      .Select(x => x.LikedUserId)
      .ToListAsync();
  
  private static Expression<Func<UserLike, bool>> LikingUserIs(int userId) => like => like.LikingUserId == userId;
  private static Expression<Func<UserLike, bool>> LikedUserIs(int userId) => like => like.LikedUserId == userId;
  private IQueryable<UserLike> ReadOnlyLikes => db.Likes.AsNoTracking();
}

file static class LikeRepositoryExtensions {
  public static IQueryable<User> FilterUsers(this IQueryable<UserLike> likes, LikedListType type, int userId) => type switch {
    LikedListType.LikedBy => likes.Where(like => like.LikedUserId == userId).Select(x => x.LikingUser),
    LikedListType.Liked => likes.Where(like => like.LikingUserId == userId).Select(x => x.LikedUser),
    // tough shit
    LikedListType.Mutual => likes
      .Where(like => like.LikingUserId == userId)
      .Join(likes,
        l => new { A = l.LikingUserId, B = l.LikedUserId },
        r => new { A = r.LikedUserId, B = r.LikingUserId },
        (l, r) => l.LikedUser
      ),
    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
  };
}
