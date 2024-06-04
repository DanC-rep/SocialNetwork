using Logic.Interfaces;
using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Services
{
	public class UserService
	{
		private readonly IUserRepository userRepository;
		private readonly UserManager<User> userManager;

		public UserService(IUserRepository usrRepo, UserManager<User> usrMgr)
		{
			userRepository = usrRepo;
			userManager = usrMgr;
		}

		public User CreateUser(RegisterViewModel _user)
		{
			return new User
			{
				Email = _user.Email,
				Name = _user.Name,
				Surname = _user.Surname,
				Patronymic = _user.Patronymic,
				BirthDate = _user.BirthDate,
				City = _user.City,
				Country = _user.Country,
				Gender = _user.Gender,
				UserName = _user.Email
			};
		}

		public ProfileInfoViewModel GetProfileInfo(User user)
		{
			return new ProfileInfoViewModel
			{
				Name = user.Name,
				Surname = user.Surname,
				Patronymic = user.Patronymic,
				City = user.City,
				Country = user.Country,
				BirthDate = user.BirthDate.ToString("MM.dd.yyyy"),
				Gender = user.Gender
			};
		}

		public async Task<User> GetByEmail(string email)
		{
			return await userRepository.GetByEmail(email);
		}

		public async Task<User> GetById(string id)
		{
			return await userRepository.GetById(id);
		}

		public async Task<User> GetAuthUserInfo(ClaimsPrincipal user)
		{
			return await userRepository.GetAuthUserInfo(user);
		}
	}
}
