using API.Extensions;
using API.Services.Abstractions.Repositories;

using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers;

// do I really need to use Unit of Work here?
public class UserActivityTracker : IAsyncActionFilter {
  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
    var result = await next();
    if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true) return;
    var id = result.HttpContext.User.GetId();
    var repo = result.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
    await repo.UpdateLastActiveAsync(id);
  }
}
