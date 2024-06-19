using Application.Services;
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
		
		[HttpGet]
		public IActionResult Index()
		{
			IEnumerable<ProfileInfoViewModel> users;
			if (User.Identity.IsAuthenticated)
			{
				users = userService.GetAllUsers().Where(u => u.Email != User.Identity.Name).Select(u => userService.GetProfileInfo(u));
			}
			else
			{
				users = userService.GetAllUsers().Select(u => userService.GetProfileInfo(u));
			}

			return View(users);
		}
	}
}