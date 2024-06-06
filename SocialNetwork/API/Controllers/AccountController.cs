using Application.Services;
using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<User> signInManager;
		private readonly UserManager<User> userManager;
		private readonly UserService userService;

		public AccountController(SignInManager<User> signInMgr, UserService usrService,
			UserManager<User> usrMgr)
		{
			signInManager = signInMgr;
			userService = usrService;
			userManager = usrMgr;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = userService.CreateUser(model);
				var result = await userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
					{
						return RedirectToAction("UsersList", "Users");
					}

					await signInManager.SignInAsync(user, false);
					return RedirectToAction("Index", "Home");
				}

				AddErrorsFromResult(result);
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult Login(string? returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
		{
			var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

			if (result.Succeeded)
			{
				if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
				{
					return Redirect(returnUrl);
				}

				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError(string.Empty, "Неправильный логин или пароль");
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> EditProfile(string id)
		{
			var user = await userService.GetById(id);

			if (user != null)
			{
				return View(userService.CreateEditProfileDTO(user));
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> EditProfile(EditProfileViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await userService.EditProfile(model);

				if (result.Succeeded)
				{
					return RedirectToAction("UsersList", "Users");
				}
				AddErrorsFromResult(result);
			}

			return View(model);
		}

		private void AddErrorsFromResult(IdentityResult result)
		{
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
		}
	}
}
