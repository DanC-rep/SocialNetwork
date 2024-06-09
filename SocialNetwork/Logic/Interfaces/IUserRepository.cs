using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace Logic.Interfaces
{
	public interface IUserRepository
	{
		Task<IdentityResult> Add(User user, string password);
		Task<IdentityResult> Edit(EditProfileViewModel user);
		Task<IdentityResult> Delete(string id);
		Task<User> GetById(string id);
		Task<User> GetByEmail(string email);
		Task<User> Get(ClaimsPrincipal user);
		Task<User> GetAuthUserInfo(ClaimsPrincipal user);
		IEnumerable<User> GetAllUsers();
		Task<IList<string>> GetUserRoles(User user);
		Task AddUserToRoles(User user, IEnumerable<string> roles);
		Task RemoveUserFromRoles(User user, IEnumerable<string> roles);
		Task<bool> CheckPassword(User user, string password);
		Task<int> GetAttempsLoginLeft(User user);
		Task<bool> CheckUserLockedOut(User user);
		Task RemoveUserAuthToken(User user);
		Task SetUserLockoutEndDate(User user);
	}
}
