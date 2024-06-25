using Logic.Enums;
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
		public DbSet<Reaction> Reactions { get; set; }
		public DbSet<ReactionToFile> ReactionsToFiles { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<CommentPhoto> PhotosComments { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Friends>().HasKey(f => new { f.UserId, f.FriendId });
			builder.Entity<ReactionToFile>().HasKey(r => new { r.UserId, r.FileId });
			builder.Entity<CommentPhoto>().HasKey(c => new { c.CommentId, c.FileId });

			builder.Entity<ReactionToFile>().HasOne(r => r.File)
				.WithMany()
				.HasForeignKey(r => r.FileId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.Entity<ReactionToFile>().HasOne(r => r.User)
				.WithMany()
				.HasForeignKey(r => r.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.Entity<Friends>().HasOne(f => f.Friend)
				.WithMany()
				.HasForeignKey(f => f.FriendId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.Entity<Friends>().HasOne(f => f.User)
				.WithMany()
				.HasForeignKey(f => f.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.Entity<Notification>().HasOne(f => f.Sender)
				.WithMany()
				.HasForeignKey(f => f.SenderId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.Entity<Notification>().HasOne(f => f.Receiver)
				.WithMany()
				.HasForeignKey(f => f.ReceiverId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.Entity<CommentPhoto>().HasOne(c => c.File)
			.WithMany()
			.HasForeignKey(c => c.FileId)
			.OnDelete(DeleteBehavior.ClientSetNull);

			builder.Entity<Reaction>().HasData(
				new Reaction
				{
					Id = 1,
					ReactionType = ReactionType.Like,
					Path = "images/like.png"
				},
				new Reaction
				{
					Id = 2,
					ReactionType = ReactionType.Dislike,
					Path = "images/dislike.png"
				},
				new Reaction
				{
					Id = 3,
					ReactionType = ReactionType.Angry,
					Path = "images/angry.png"
				},
				new Reaction
				{
					Id = 4,
					ReactionType = ReactionType.Heart,
					Path = "images/heart.png"
				},
				new Reaction
				{
					Id = 5,
					ReactionType = ReactionType.Cry,
					Path = "images/cry.png"
				},
				new Reaction
				{
					Id = 6,
					ReactionType = ReactionType.Laugh,
					Path = "images/laugh.png"
				},
				new Reaction
				{
					Id = 7,
					ReactionType = ReactionType.Surprised,
					Path = "images/surprised.png"
				}
			);
		}
	}
}
