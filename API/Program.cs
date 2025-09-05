using API.Extensions;
using API.Helpers;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration)
                .AddIdentityServices(builder.Configuration);        

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.ExecuteAction(PreLaunchActions.ApplyMigrations);
await app.ExecuteAction(PreLaunchActions.SeedUsers);

await app.RunAsync();
