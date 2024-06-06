using Microsoft.AspNetCore.Identity;

namespace Logic.ViewModels
{
	public class EditUserRolesViewModel
	{
		public string UserId { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public IList<string> UserRoles { get; set; }
		public IEnumerable<IdentityRole> AllRoles { get; set; }
	}
}
