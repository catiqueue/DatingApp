using API.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

builder.Services.AddCors();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(app => app.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.MapControllers();

await app.RunAsync();
