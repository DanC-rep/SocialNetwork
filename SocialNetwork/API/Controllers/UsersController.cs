using Application.Services;
using Logic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private readonly UserService userService;
		private readonly RoleService roleService;

		public UsersController(UserService usrService, RoleService _roleSerivce)
		{
			userService = usrService;
			roleService = _roleSerivce;
		}

		[HttpGet]
		public IActionResult UsersList()
		{
			return View(userService.GetAllUsers());
		}

		[HttpGet]
		public async Task<IActionResult> EditUserRoles(string id)
		{
			var user = await userService.GetById(id);

			if (user != null)
			{
				var userRoles = await userService.GetUserRoles(user);
				var allRoles = roleService.GetAllRoles();

				EditUserRolesViewModel model = new EditUserRolesViewModel
				{
					UserId = user.Id,
					Email = user.Email ?? string.Empty,
					UserRoles = userRoles,
					AllRoles = allRoles
				};
				return View(model);
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> EditUserRoles(string id, List<string> roles)
		{
			var user = await userService.GetById(id);

			if (user != null)
			{
				var userRoles = await userService.GetUserRoles(user);
				var addedRoles = roles.Except(userRoles);
				var removedRoles = userRoles.Except(roles);

				await userService.AddUserToRoles(user, addedRoles);
				await userService.RemoveUserFromRoles(user, removedRoles);

				return RedirectToAction("UsersList");
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var result = await userService.DeleteUser(id);

			if (result.Succeeded)
			{
				return RedirectToAction("UsersList");
			}

			return NotFound(); // TODO Проверить работу метода, а то ошибку выдает при удалении
		}
	}
}
