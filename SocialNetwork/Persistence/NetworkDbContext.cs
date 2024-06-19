using Logic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
	public class NetworkDbContext : IdentityDbContext<User>
	{
		public NetworkDbContext(DbContextOptions<NetworkDbContext> options) : base(options) { }

		public DbSet<FileModel> Files { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<Friends> Friends { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
			base.OnModelCreating(builder);

			builder.Entity<Friends>().HasKey(f => new { f.UserId, f.FriendId });

			builder.Entity<Friends>().HasOne(f => f.Friend)
				.WithMany()
				.HasForeignKey(f => f.FriendId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.Entity<Friends>().HasOne(f => f.User)
				.WithMany()
				.HasForeignKey(f => f.UserId)
				.OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>().HasOne(f => f.Sender)
                .WithMany()
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>().HasOne(f => f.Receiver)
                .WithMany()
                .HasForeignKey(f => f.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
