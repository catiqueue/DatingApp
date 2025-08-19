namespace API.Services.Abstractions.PhotoService;

public class PhotoUploadError(string message) : Exception(message);
