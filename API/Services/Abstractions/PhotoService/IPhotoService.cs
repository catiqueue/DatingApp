using API.Helpers;

namespace API.Services.Abstractions.PhotoService;

public interface IPhotoService {
  Task<PhotoUploadResult> UploadPhotoAsync(IFormFile file);
  Task<PhotoDeletionError?> DeletePhotoAsync(string photoId);
}
