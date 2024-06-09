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
		
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UserProfile(string id)
		{
			User user = await userService.GetById(id);

			if (user != null)
			{
				ProfileInfoViewModel profile = userService.GetProfileInfo(user); 
				return View(profile);
			}
			return NotFound();
		}
	}
}