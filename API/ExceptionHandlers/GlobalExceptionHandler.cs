using System.Text.Json;

using API.DTO.Responses;

using Microsoft.AspNetCore.Diagnostics;

namespace API.ExceptionHandlers;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env) : IExceptionHandler {
  private static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
  
  public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken) {
    logger.LogError(exception, "Caught an exception of type {Name}: {Message}", exception.GetType().Name, exception.Message);
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = 500;
    var response = env.IsDevelopment() 
      ? new ApiErrorResponse(context.Response.StatusCode, exception.Message, exception.StackTrace) 
      : new ApiErrorResponse(context.Response.StatusCode, exception.Message, "Internal Server Error");
      
    var json = JsonSerializer.Serialize(response, SerializerOptions);
    await context.Response.WriteAsync(json, cancellationToken);
    return true;
  }
}
