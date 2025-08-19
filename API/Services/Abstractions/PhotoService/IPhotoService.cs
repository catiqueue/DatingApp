using DotNext;

namespace API.Services.Abstractions.PhotoService;

public interface IPhotoService {
  Task<Result<PhotoUploadInfo>> UploadPhotoAsync(IFormFile file);
  Task<Optional<PhotoDeletionError>> DeletePhotoAsync(string photoId);
}
