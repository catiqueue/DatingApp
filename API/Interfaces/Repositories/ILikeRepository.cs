using API.DTO.Responses;
using API.Entities;
using API.Helpers;
using API.Repositories;

namespace API.Interfaces.Repositories;

public interface ILikeRepository : IRepository {
  Task<int> CountAsync(LikedListType request, int userId);
  void AddLike(UserLike like);
  void DeleteLike(UserLike like);
  Task<UserLike?> GetLikeAsync(int likingUserId, int likedUserId);
  Task<IEnumerable<UserDto>> GetUserDtoLikedListAsync(Page page, LikedListType request, int userId);
  Task<IEnumerable<int>> GetUsersLikedIdsAsync(int currentUserId);
}
