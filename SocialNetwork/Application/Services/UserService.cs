using Logic.Enums;
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
		private readonly FriendsService friendsService;

		public UserService(IUserRepository usrRepo, FriendsService _friendsService)
		{
			userRepository = usrRepo;
			friendsService = _friendsService;
		}

		public User CreateUser(RegisterViewModel _user)
		{
			return new User
			{
				Email = _user.Email,
				Name = _user.Name,
				Surname = _user.Surname,
				Patronymic = _user.Patronymic,
				BirthDate = _user.BirthDate.GetValueOrDefault(),
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
				Id = user.Id,
				Name = user.Name,
				Surname = user.Surname,
				Patronymic = user.Patronymic,
				City = user.City,
				Country = user.Country,
				BirthDate = user.BirthDate.ToString("MM.dd.yyyy"),
				Gender = user.Gender,
				FriendsCount = friendsService.GetFriendsByRelation(user.Id, RelationType.Friend).Count(),
				FollowersCount = friendsService.GetFriendsByRelation(user.Id, RelationType.Following).Count()
			};
		}

		public EditProfileViewModel CreateEditProfileDTO(User user)
		{
			return new EditProfileViewModel
			{
				Id = user.Id,
				Name = user.Name,
				Surname = user.Surname,
				Patronymic = user.Patronymic,
				City = user.City,
				Country = user.Country,
				BirthDate = user.BirthDate,
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

		public async Task<User> Get(ClaimsPrincipal user)
		{
			return await userRepository.Get(user);
		}

		public IEnumerable<User> GetAllUsers()
		{
			return userRepository.GetAllUsers();
		}

		public async Task<IList<string>> GetUserRoles(User user)
		{
			return await userRepository.GetUserRoles(user);
		}

		public async Task AddUserToRoles(User user, IEnumerable<string> roles)
		{
			await userRepository.AddUserToRoles(user, roles);
		}

		public async Task RemoveUserFromRoles(User user, IEnumerable<string> roles)
		{
			await userRepository.RemoveUserFromRoles(user, roles);
		}

		public async Task<IdentityResult> EditProfile(EditProfileViewModel model)
		{
			return await userRepository.Edit(model);
		}

		public async Task<IdentityResult> DeleteUser(string id)
		{
			return await userRepository.Delete(id);
		}

		public async Task<bool> CheckPassword(User user, string password) 
		{
			return await userRepository.CheckPassword(user, password);
		}

		public async Task<IdentityResult> CreateUser(User user, string password)
		{
			return await userRepository.Add(user, password);
		}

		public async Task<int> GetAttempsLoginLeft(User user)
		{
			return await userRepository.GetAttempsLoginLeft(user);
		}
		public async Task<bool> CheckUserLockedOut(User user)
		{
			return await userRepository.CheckUserLockedOut(user);
		}

		public async Task SetUserLockoutEndDate(User user)
		{
			await userRepository.SetUserLockoutEndDate(user);
		}

		public async Task RemoveUserAuthToken(User user)
		{
			await userRepository.RemoveUserAuthToken(user);
		}
 	}
}
