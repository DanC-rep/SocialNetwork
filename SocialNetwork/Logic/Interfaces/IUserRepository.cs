using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace Logic.Interfaces
{
	public interface IUserRepository
	{
		Task<IdentityResult> Edit(EditProfileViewModel user);
		Task<IdentityResult> Delete(string id);
		Task<User> GetById(string id);
		Task<User> GetByEmail(string email);
		Task<User> GetAuthUserInfo(ClaimsPrincipal user);
		IEnumerable<User> GetAllUsers();
		Task<IList<string>> GetUserRoles(User user);
		Task AddUserToRoles(User user, IEnumerable<string> roles);
		Task RemoveUserFromRoles(User user, IEnumerable<string> roles);
	}
}
