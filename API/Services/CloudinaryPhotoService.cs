using API.Interfaces.PhotoService;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

using Microsoft.Extensions.Options;

namespace API.Services;

public sealed record CloudinaryOptions {
  public required string CloudName { get; init; }
  public required string ApiKey { get; init; }
  public required string ApiSecret { get; init; }
}

public sealed class CloudinaryPhotoService(IOptions<CloudinaryOptions> options) : IPhotoService {
  private readonly Cloudinary _cloudinary = new(new Account(options.Value.CloudName, options.Value.ApiKey, options.Value.ApiSecret));

  public async Task<PhotoUploadResult> UploadPhotoAsync(IFormFile file) {
    if (file.Length <= 0) return PhotoUploadResult.Failure(new PhotoUploadError("File does not exist"));
    await using var stream = file.OpenReadStream();
    var uploadParams = new ImageUploadParams {
      File = new FileDescription(file.FileName, stream),
      Transformation = new Transformation().Width(512).Height(512).Crop("fill").Gravity("face"),
      AssetFolder = "Users"
    };
    var response = await _cloudinary.UploadAsync(uploadParams);
    return response.Error is not { } error
      ? PhotoUploadResult.Success(new PhotoUploadInfo(response.SecureUrl.AbsoluteUri, response.PublicId))
      : PhotoUploadResult.Failure(new PhotoUploadError(error.Message));
  }

  public async Task<PhotoDeletionError?> DeletePhotoAsync(string photoId) {
    var deleteParams = new DeletionParams(photoId);
    var result = await _cloudinary.DestroyAsync(deleteParams);
    return result.Error is not { } error
      ? null
      : new PhotoDeletionError(error.Message);
  }
}
