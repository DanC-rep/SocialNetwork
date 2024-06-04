using Logic.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Persistence.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly NetworkDbContext context;
		private readonly UserManager<User> userManager;

		public UserRepository(NetworkDbContext ctx, UserManager<User> usrMgr)
		{
			context = ctx;
			userManager = usrMgr;
		}

		public async Task Delete(string id)
		{
			var user = await context.Users.SingleOrDefaultAsync(u => u.Id == id);

			if (user != null)
			{
				context.Users.Remove(user);
				await context.SaveChangesAsync();
			}
		}

		public async Task Edit(User user)
		{
			var dbEntry = await context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);

			if (dbEntry != null)
			{
				dbEntry.UserName = user.UserName;
				dbEntry.PhoneNumber = user.PhoneNumber;
				dbEntry.Name = user.Name;
				dbEntry.Surname = user.Surname;
				dbEntry.Patronymic = user.Patronymic;
				dbEntry.City = user.City;
				dbEntry.Country = user.Country;
				dbEntry.BirthDate = user.BirthDate;
				dbEntry.Gender = user.Gender;

				// прописать смену пароля (наверное отдельным методом)
			}
		}

		public async Task<User> GetByEmail(string email)
		{
			return await userManager.FindByEmailAsync(email);
		}

		public async Task<User> GetById(string id)
		{
			return await userManager.FindByIdAsync(id);
		}

		public async Task<User> GetAuthUserInfo(ClaimsPrincipal user)
		{
			return await userManager.GetUserAsync(user);
		}
	}
}
