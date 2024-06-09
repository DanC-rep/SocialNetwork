using Application.Services;
using Logic.Interfaces;
using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<User> signInManager;
		private readonly UserService userService;
		private readonly EmailService emailService;
		private readonly PasswordService passwordService;

		public AccountController(SignInManager<User> signInMgr, UserService usrService,
			EmailService _emailService, PasswordService _passRestoreService)
		{
			signInManager = signInMgr;
			userService = usrService;
			emailService = _emailService;
			passwordService = _passRestoreService;
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
				var result = await userService.CreateUser(user, model.Password);

				if (result.Succeeded)
				{
					if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
					{
						return RedirectToAction("UsersList", "Users");
					}

					await SendConfirmationEmail(model.Email, user);
					return View("RegistrationSuccessful");
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
			if (ModelState.IsValid)
			{
				var user = await userService.GetByEmail(model.Email);

				if (user != null && !user.EmailConfirmed && (await userService.CheckPassword(user, model.Password)))
				{
					ModelState.AddModelError(string.Empty, "Email ещё не подтвержден");
					return View(model);
				}

				var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

				if (result.Succeeded)
				{
					if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}

					return RedirectToAction("Index", "Home");
				}
				if (result.IsLockedOut)
				{
					await emailService.SendAccountLockedEmail(model.Email);
					return View("AccountLocked");
				}
				var attempsLeft = await userService.GetAttempsLoginLeft(user);

				ModelState.AddModelError(string.Empty, $"Неверная попытка входа в систему. Оставшиеся попытки: {attempsLeft}");
			}
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
		public async Task<IActionResult> EditProfile(string id, string? url = null)
		{
			var user = await userService.GetById(id);

			if (user != null)
			{
				ViewData["returnUrl"] = url;
				return View(userService.CreateEditProfileDTO(user));
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> EditProfile(EditProfileViewModel model, string? returnUrl)
		{
			if (ModelState.IsValid)
			{
				var result = await userService.EditProfile(model);

				if (result.Succeeded)
				{
					if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}
					return RedirectToAction("MyProfile", "Account");
				}
				AddErrorsFromResult(result);
			}

			return View(model);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> MyProfile()
		{
			User user = await userService.GetAuthUserInfo(User);

			if (user != null)
			{
				ProfileInfoViewModel profile = userService.GetProfileInfo(user);
				return View(profile);
			}

			return NotFound();
		}

		[HttpGet]
		public IActionResult RegistrationSuccessful()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmEmail(string userId, string token)
		{
			if (userId.IsNullOrEmpty() || token.IsNullOrEmpty())
			{
				ViewBag.Message = "Ссылка недействительна или срок ее действия истек.";
			}

			var user = await userService.GetById(userId);
			if (user == null)
			{
				ViewBag.ErrorMessage = $"ID пользователя {userId} неверен";
				return NotFound();
			}

			var result = await emailService.ConfirmEmail(user, token);

			ViewBag.Message = result.Succeeded ? "Спасибо за подтверждение почты" : "Почта не может быть подтверждена";

			return View();
		}

		[HttpGet]
		public IActionResult ResendConfirmationEmail(bool isResend = true)
		{
			if (isResend)
			{
				ViewBag.Message = "Повторная отправка подтверждения Email";
			}
			else
			{
				ViewBag.Message = "Отправка подтверждения Email";
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResendConfirmationEmail(string email)
		{
			var user = await userService.GetByEmail(email);

			if (user == null || await emailService.CheckConfirmedEmail(user))
			{
				return View("ConfirmationEmailSent");
			}

			await SendConfirmationEmail(email, user);
			return View("ConfirmationEmailSent");
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userService.GetByEmail(model.Email);

				if (user != null && await emailService.CheckConfirmedEmail(user))
				{
					await SendForgotPasswordEmail(user.Email ?? "", user);
				}
				return RedirectToAction("ForgotPasswordConfirmation");
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ResetPassword(string token, string email)
		{
			if (token.IsNullOrEmpty() || email.IsNullOrEmpty())
			{
				ViewBag.ErrorTitle = "Неверный токен сброса пароля";
				ViewBag.ErrorMessage = "Ссылка устарела или недействительна";
				return NotFound();
			}
			RestorePasswordViewModel model = new RestorePasswordViewModel();
			model.Token = token;
			model.Email = email;
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(RestorePasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userService.GetByEmail(model.Email);

				if (user != null)
				{
					var result = await passwordService.ResetPassword(user, model.Token, model.Password);

					if (result.Succeeded)
					{
						if (await userService.CheckUserLockedOut(user))
						{
							await userService.SetUserLockoutEndDate(user);
						}
						await userService.RemoveUserAuthToken(user);

						return RedirectToAction("ResetPasswordConfirmation");
					}

					AddErrorsFromResult(result);
					return View(model);
				}
				
				return RedirectToAction("ResetPasswordConfirmation");
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		[HttpGet]
		[Authorize]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userService.Get(User);

				if (user == null)
				{
					return RedirectToAction("Login");
				}

				var result = await passwordService.ChangePassword(user, model.OldPassword, model.NewPassword);

				if (result.Succeeded)
				{
					await signInManager.RefreshSignInAsync(user);
					return RedirectToAction("ChangePasswordConfirmation");
				}

				AddErrorsFromResult(result);
				return View();
			}
			return View(model);
		}

		[HttpGet]
		[Authorize]
		public IActionResult ChangePasswordConfirmation()
		{
			return View();
		}

		private void AddErrorsFromResult(IdentityResult result)
		{
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
		}

		private async Task SendConfirmationEmail(string email, User user)
		{
			var token = await emailService.CreateConfirmToken(user);

			var confirmationLink = Url.Action("ConfirmEmail", "Account", new { UserId = user.Id, Token = token }, protocol: HttpContext.Request.Scheme);
			await emailService.SendEmail(email, confirmationLink ?? "");
		}

		private async Task SendForgotPasswordEmail(string email, User user)
		{
			var token = await passwordService.CreatePasswordRestoreToken(user);

			var passResetLink = Url.Action("ResetPassword", "Account", new { Email = email, Token = token }, protocol: HttpContext.Request.Scheme);
			await passwordService.SendEmail(email, passResetLink ?? "");
		}
	}
}
