using API.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data;

public sealed class DataContext(DbContextOptions<DataContext> settings) : DbContext(settings) {
  public DbSet<DbUser> Users { get; set; }
  public DbSet<DbUserLike> Likes { get; set; }

  protected override void OnModelCreating(ModelBuilder builder) {
    base.OnModelCreating(builder);
    
    builder.Entity<DbUser>()
      .Property(u => u.Gender)
      .HasConversion(new EnumToStringConverter<UserGender>());

    builder.Entity<DbUserLike>()
      .HasKey(k => new { k.LikingUserId, k.LikedUserId });
    
    builder.Entity<DbUserLike>()
      .HasOne(like => like.LikingUser)
      .WithMany(user => user.Likes)
      .HasForeignKey(like => like.LikingUserId)
      .OnDelete(DeleteBehavior.Cascade);
    
    builder.Entity<DbUserLike>()
      .HasOne(like => like.LikedUser)
      .WithMany(user => user.LikedBy)
      .HasForeignKey(like => like.LikedUserId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
