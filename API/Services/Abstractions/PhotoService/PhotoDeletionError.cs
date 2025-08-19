namespace API.Services.Abstractions.PhotoService;

public class PhotoDeletionError(string message) : Exception(message);
