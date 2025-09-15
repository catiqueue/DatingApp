using API.Entities;
using API.Extensions;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data;

public sealed class ApiDbContext(DbContextOptions<ApiDbContext> settings, IHttpContextAccessor httpAccessor) 
  : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>(settings) {
  private int? QueryingUserId { get; } = httpAccessor.HttpContext?.User.GetIdSafe();
  public DbSet<Photo> Photos { get; set; }
  public DbSet<UserLike> Likes { get; set; }
  public DbSet<Message> Messages { get; set; }
  public DbSet<Group> Groups { get; set; }
  public DbSet<GroupConnection> Connections { get; set; }

  protected override void OnModelCreating(ModelBuilder builder) {
    base.OnModelCreating(builder);

    builder.Entity<User>()
      .HasMany<Photo>(user => user.Photos)
      .WithOne(photo => photo.User)
      .HasForeignKey(photo => photo.UserId)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);
    
    builder.Entity<User>()
      .HasQueryFilter(user => user.NormalizedUserName != "ADMIN");
    
    builder.Entity<Photo>()
      .HasOne(photo => photo.User)
      .WithMany(user => user.Photos)
      .HasForeignKey(photo => photo.UserId)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);

    builder.Entity<Photo>()
      .HasQueryFilter(photo => photo.IsApproved || photo.UserId == QueryingUserId);

    builder.Entity<UserRole>()
      .HasOne(join => join.User)
      .WithMany(user => user.UserRoles)
      .HasForeignKey(join => join.UserId)
      .IsRequired();

    builder.Entity<UserRole>()
      .HasOne(join => join.Role)
      .WithMany(role => role.UserRoles)
      .HasForeignKey(join => join.RoleId)
      .IsRequired();
    
    builder.Entity<User>()
      .Property(u => u.Gender)
      .HasConversion(new EnumToStringConverter<Gender>());

    builder.Entity<UserLike>()
      .HasKey(k => new { k.LikingUserId, k.LikedUserId });
    
    builder.Entity<UserLike>()
      .HasOne(like => like.LikingUser)
      .WithMany(user => user.Likes)
      .HasForeignKey(like => like.LikingUserId)
      .OnDelete(DeleteBehavior.Cascade);
    
    builder.Entity<UserLike>()
      .HasOne(like => like.LikedUser)
      .WithMany(user => user.LikedBy)
      .HasForeignKey(like => like.LikedUserId)
      .OnDelete(DeleteBehavior.Cascade);
    
    builder.Entity<Message>()
      .HasOne(message => message.Sender)
      .WithMany(user => user.MessagesSent)
      .OnDelete(DeleteBehavior.Restrict);
    
    builder.Entity<Message>()
      .HasOne(message => message.Recipient)
      .WithMany(user => user.MessagesReceived)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
