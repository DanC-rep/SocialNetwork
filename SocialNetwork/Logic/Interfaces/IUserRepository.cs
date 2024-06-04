using Logic.Models;
using System.Security.Claims;

namespace Logic.Interfaces
{
	public interface IUserRepository
	{
		Task Edit(User user);
		Task Delete(string id);
		Task<User> GetById(string id);
		Task<User> GetByEmail(string email);
		Task<User> GetAuthUserInfo(ClaimsPrincipal user);
	}
}
