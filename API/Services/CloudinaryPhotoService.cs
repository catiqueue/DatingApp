using API.Services.Abstractions.PhotoService;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

using DotNext;

using Microsoft.Extensions.Options;

namespace API.Services;

public sealed record CloudinaryOptions {
  public required string CloudName { get; init; }
  public required string ApiKey { get; init; }
  public required string ApiSecret { get; init; }
}

public sealed class CloudinaryPhotoService(IOptions<CloudinaryOptions> options) : IPhotoService {
  private readonly Cloudinary _cloudinary = new(new Account(options.Value.CloudName, options.Value.ApiKey, options.Value.ApiSecret));

  public async Task<Result<PhotoUploadInfo>> UploadPhotoAsync(IFormFile file) {
    if (file.Length <= 0) return Result.FromException<PhotoUploadInfo>(new PhotoUploadError("File does not exist"));
    await using var stream = file.OpenReadStream();
    var uploadParams = new ImageUploadParams {
      File = new FileDescription(file.FileName, stream),
      Transformation = new Transformation().Width(512).Height(512).Crop("fill").Gravity("face"),
      AssetFolder = "Users"
    };
    var response = await _cloudinary.UploadAsync(uploadParams);
    return response.Error is not { } error
      ? Result.FromValue(new PhotoUploadInfo(response.SecureUrl.AbsoluteUri, response.PublicId))
      : Result.FromException<PhotoUploadInfo>(new PhotoUploadError(error.Message));
  }

  public async Task<Optional<PhotoDeletionError>> DeletePhotoAsync(string photoId) {
    var deleteParams = new DeletionParams(photoId);
    var result = await _cloudinary.DestroyAsync(deleteParams);
    return result.Error is not { } error
      ? Optional.None<PhotoDeletionError>()
      : Optional.Some(new PhotoDeletionError(error.Message));
  }
}
