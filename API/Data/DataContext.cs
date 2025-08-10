using API.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data;

public sealed class DataContext(DbContextOptions<DataContext> settings) : DbContext(settings) {
  public DbSet<DbUser> Users { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<DbUser>()
      .Property(u => u.Gender)
      .HasConversion(new EnumToStringConverter<UserGender>());
  }
}
