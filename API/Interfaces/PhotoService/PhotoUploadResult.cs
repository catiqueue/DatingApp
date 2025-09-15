using API.Helpers;

namespace API.Interfaces.PhotoService;

public record PhotoUploadResult : Result<PhotoUploadInfo, PhotoUploadError> {
  public PhotoUploadResult(PhotoUploadInfo value) : base(value) { }
  public PhotoUploadResult(PhotoUploadError error) : base(error) { }
  public new static PhotoUploadResult Success(PhotoUploadInfo value) => new(value);
  public new static PhotoUploadResult Failure(PhotoUploadError error) => new(error);
}
