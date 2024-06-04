using Application.Services;
using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class HomeController : Controller
	{
		private readonly UserService userService;

		public HomeController(UserService usrService)
		{
			userService = usrService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Profile(string? id = null)
		{
			User user; // заменить на viewmodel какую нибудь
			if (id is null)
			{
				user = await userService.GetAuthUserInfo(User);
			}
			else
			{
				user = await userService.GetById(id);
			}

			if (user != null)
			{
				ProfileInfoViewModel profile = userService.GetProfileInfo(user); 
				return View(profile);
			}
			return View("Index");
		}
	}
}
