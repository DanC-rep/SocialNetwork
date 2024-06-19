using Logic.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Logic.ViewModels;

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

		public async Task<IdentityResult> Add(User user, string password)
		{
			return await userManager.CreateAsync(user, password);
		}
		public async Task<IdentityResult> Delete(string id)
		{
			var user = await userManager.FindByIdAsync(id);

			if (user != null)
			{
				return await userManager.DeleteAsync(user);
			}

			return IdentityResult.Failed();
		}

		public async Task<IdentityResult> Edit(EditProfileViewModel user)
		{
			var dbEntry = await userManager.FindByIdAsync(user.Id);

			if (dbEntry != null)
			{
				dbEntry.Name = user.Name;
				dbEntry.Surname = user.Surname;
				dbEntry.Patronymic = user.Patronymic;
				dbEntry.City = user.City;
				dbEntry.Country = user.Country;
				dbEntry.BirthDate = user.BirthDate.GetValueOrDefault();
				dbEntry.Gender = user.Gender;

				return await userManager.UpdateAsync(dbEntry);
			}

			return IdentityResult.Failed();
		}

		public async Task<User> GetByEmail(string email)
		{
			return await userManager.FindByEmailAsync(email) ?? new User();
		}

		public async Task<User> GetById(string id)
		{
			return await userManager.FindByIdAsync(id);
		}

		public async Task<User> Get(ClaimsPrincipal user)
		{
			return await userManager.GetUserAsync(user);
		}

		public IEnumerable<User> GetAllUsers()
		{
			return userManager.Users;
		}

		public async Task<IList<string>> GetUserRoles(User user)
		{
			return await userManager.GetRolesAsync(user);
		}

		public async Task AddUserToRoles(User user, IEnumerable<string> roles)
		{
			await userManager.AddToRolesAsync(user, roles);
		}

		public async Task RemoveUserFromRoles(User user, IEnumerable<string> roles)
		{
			await userManager.RemoveFromRolesAsync(user, roles);
		}

		public async Task<bool> CheckPassword(User user, string password)
		{
			return await userManager.CheckPasswordAsync(user, password);
		}

		public async Task<int> GetAttempsLoginLeft(User user)
		{
			return userManager.Options.Lockout.MaxFailedAccessAttempts - await userManager.GetAccessFailedCountAsync(user);
		}

		public async Task<bool> CheckUserLockedOut(User user)
		{
			return await userManager.IsLockedOutAsync(user);
		}

		public async Task SetUserLockoutEndDate(User user)
		{
			await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
		}

		public async Task RemoveUserAuthToken(User user)
		{
			await userManager.RemoveAuthenticationTokenAsync(user, "ResetPassword", "ResetPasswordToken");
		}
	}
}
