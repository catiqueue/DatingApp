using API.Entities;

using Microsoft.EntityFrameworkCore;

namespace API.Data {
  public sealed class DataContext(DbContextOptions<DataContext> settings) : DbContext(settings) {
    public DbSet<AppUser> Users { get; set; }
  }
}
