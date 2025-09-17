using API.Controllers;
using API.Extensions;
using API.Helpers;
using API.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration)
                .AddIdentityServices(builder.Configuration);        

var app = builder.Build();

// app.UseMiddleware<ExceptionMiddleware>(); // replaced with IExceptionHandler
// app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.MapFallbackToController(nameof(FallbackController.Index), nameof(FallbackController)[..^10]);

await app.ExecuteAction(PreLaunchActions.ApplyMigrations);
await app.ExecuteAction(PreLaunchActions.SeedUsers);
await app.ExecuteAction(PreLaunchActions.ClearConnections);

await app.RunAsync();
