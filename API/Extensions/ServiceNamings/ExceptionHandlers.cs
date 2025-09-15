using API.ExceptionHandlers;

namespace API.Extensions;

public static partial class ServiceNamings {
  public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    => services.AddExceptionHandler<GlobalExceptionHandler>();
}
