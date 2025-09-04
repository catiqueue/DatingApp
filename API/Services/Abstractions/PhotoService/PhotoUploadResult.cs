using API.Helpers;

namespace API.Services.Abstractions.PhotoService;

public class PhotoUploadResult : Result<PhotoUploadInfo, PhotoUploadError> {
  internal new static PhotoUploadResult Success(PhotoUploadInfo value) => new() { IsSuccess = true, Value = value };
  internal new static PhotoUploadResult Failure(PhotoUploadError error) => new() { IsSuccess = false, Error = error };
}
