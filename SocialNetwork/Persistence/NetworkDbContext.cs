using Logic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
	public class NetworkDbContext : IdentityDbContext<User>
	{
		public NetworkDbContext(DbContextOptions<NetworkDbContext> options) : base(options) { }

		public DbSet<FileModel> Files { get; set; }
	}
}
