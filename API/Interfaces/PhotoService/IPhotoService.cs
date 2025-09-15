namespace API.Interfaces.PhotoService;

public interface IPhotoService {
  Task<PhotoUploadResult> UploadPhotoAsync(IFormFile file);
  Task<PhotoDeletionError?> DeletePhotoAsync(string photoId);
}
