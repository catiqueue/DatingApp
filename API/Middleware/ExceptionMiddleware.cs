using System.Net;
using System.Text.Json;

using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env) {
  private static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
  
  public async Task InvokeAsync(HttpContext context) {
    try {
      await next(context);
    }
    catch (Exception e)
    {
      logger.LogError(e, "Caught an exception of type {Name}: {Message}", e.GetType().Name, e.Message);
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = 500;
      var response = env.IsDevelopment() 
        ? new ApiError(context.Response.StatusCode, e.Message, e.StackTrace) 
        : new ApiError(context.Response.StatusCode, e.Message, "Internal Server Error");
      
      var json = JsonSerializer.Serialize(response, SerializerOptions);
      await context.Response.WriteAsync(json);
    }
  }
}
