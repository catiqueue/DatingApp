using API.Data.Repositories;
using API.Data.Responses;
using API.Entities;
using API.Helpers;

namespace API.Services.Abstractions;

public interface ILikesRepository {
  Task<bool> TrySaveAllAsync();
  Task<int> CountAsync(LikedListType request, uint userId);
  void AddLike(DbUserLike like);
  void DeleteLike(DbUserLike like);
  Task<DbUserLike?> GetDbLikeAsync(uint likingUserId, uint likedUserId);
  Task<IEnumerable<SimpleUser>> GetSimpleUserLikedListAsync(Page page, LikedListType request, uint userId);
  Task<IEnumerable<uint>> GetUsersLikedIdsAsync(uint currentUserId);
}
