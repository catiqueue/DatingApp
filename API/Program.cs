using API.Extensions;
using API.Helpers;
using API.Middleware;
using API.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration)
                .AddIdentityServices(builder.Configuration);        

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
// app.MapHub<LikeHub>("hubs/like");

await app.ExecuteAction(PreLaunchActions.ApplyMigrations);
await app.ExecuteAction(PreLaunchActions.SeedUsers);
await app.ExecuteAction(PreLaunchActions.ClearConnections);

await app.RunAsync();
