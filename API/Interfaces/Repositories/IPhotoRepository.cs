using API.DTO.Responses;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.Repositories;

public interface IPhotoRepository : IRepository {
  Task<int> CountAsync(PhotoFilter filter);
  Task<IEnumerable<UnapprovedPhotoDto>> GetUnapprovedPhotosAsync(Page page);
  Task<bool> TryApprovePhotoAsync(int photoId);
  Task<Photo?> GetPhotoAsync(int photoId);
  void DeletePhoto(Photo photo);
}
