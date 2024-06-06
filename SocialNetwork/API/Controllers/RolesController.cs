using Application.Services;
using Logic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Authorize(Roles = "Admin")]
	public class RolesController : Controller
	{
		private readonly RoleService roleService;

		public RolesController(RoleService _roleService)
		{
			roleService = _roleService;
		}

		[HttpGet]
		public IActionResult RolesList()
		{
			return View(roleService.GetAllRoles());
		}

		[HttpGet]
		public IActionResult CreateRole()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
		{
			if (ModelState.IsValid)
			{
				bool roleExists = await roleService.RoleExists(model.Role);
				if (roleExists)
				{
					ModelState.AddModelError("", "Такая роль уже существует");
				}
				else
				{
					IdentityResult result = await roleService.AddRole(model);

					if (result.Succeeded)
					{
						return RedirectToAction("RolesList");
					}

					foreach (IdentityError error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteRole(string id)
		{
			IdentityResult result = await roleService.DeleteRole(id);

			if (result.Succeeded)
			{
				return RedirectToAction("RolesList");
			}
			return NotFound();
		}
	}
}
