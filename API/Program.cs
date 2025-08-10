using API.Data;
using API.Extensions;
using API.Middleware;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(app => app.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var scope = app.Services.CreateScope();
try {
  var services = scope.ServiceProvider;
  var context = services.GetRequiredService<DataContext>();
  await context.Database.MigrateAsync();
  await Seed.SeedUsers(context);
} catch (Exception ex) {
  var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
  logger.LogError(ex, "An error occurred during migration");
} finally {
  scope.Dispose(); 
}

await app.RunAsync();
