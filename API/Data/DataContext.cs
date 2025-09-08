using API.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data;

public sealed class DataContext(DbContextOptions<DataContext> settings) 
  : IdentityDbContext<DbUser, DbRole, uint, IdentityUserClaim<uint>, DbUserRole, IdentityUserLogin<uint>, IdentityRoleClaim<uint>, IdentityUserToken<uint>>(settings) {
  public DbSet<DbUserLike> Likes { get; set; }
  public DbSet<DbMessage> Messages { get; set; }
  public DbSet<DbGroup> Groups { get; set; }
  public DbSet<DbGroupConnection> Connections { get; set; }

  protected override void OnModelCreating(ModelBuilder builder) {
    base.OnModelCreating(builder);

    builder.Entity<DbUserRole>()
      .HasOne(join => join.User)
      .WithMany(user => user.UserRoles)
      .HasForeignKey(join => join.UserId)
      .IsRequired();

    builder.Entity<DbUserRole>()
      .HasOne(join => join.Role)
      .WithMany(role => role.UserRoles)
      .HasForeignKey(join => join.RoleId)
      .IsRequired();
    
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
    
    builder.Entity<DbMessage>()
      .HasOne(message => message.Sender)
      .WithMany(user => user.MessagesSent)
      .OnDelete(DeleteBehavior.Restrict);
    
    builder.Entity<DbMessage>()
      .HasOne(message => message.Recipient)
      .WithMany(user => user.MessagesReceived)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
