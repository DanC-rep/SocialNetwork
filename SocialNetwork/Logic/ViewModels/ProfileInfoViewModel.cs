using Logic.Enums;
using Logic.Models;
using System.ComponentModel.DataAnnotations;

namespace Logic.ViewModels
{
	public class ProfileInfoViewModel
	{
		public string Id { get; set; } = string.Empty;

		[Display(Name = "Имя")]
		public string Name { get; set; } = string.Empty;

		[Display(Name = "Фамилия")]
		public string Surname { get; set; } = string.Empty;

		[Display(Name = "Отчество")]
		public string? Patronymic { get; set; } = string.Empty;

		[Display(Name = "Город")]
		public string City { get; set; } = string.Empty;

		[Display(Name = "Страна")]
		public string Country { get; set; } = string.Empty;

		[Display(Name = "Дата рождения")]
		public string BirthDate { get; set; }

		[Display(Name = "Пол")]
		public Gender Gender { get; set; }

		[Display(Name = "Друзья")]
		public int FriendsCount { get; set; }

		[Display(Name = "Подписчики")]
		public int FollowersCount { get; set; }

		public RelationType RelationType { get; set; } = RelationType.None;

		public string RelationTypeRu 
		{
			get
			{
				switch (RelationType)
				{
					case RelationType.Friend:
						return "В друзьях";
					case RelationType.Follower:
						return "Подписчик";
					default:
						return string.Empty;
				}
			}
		}

		public string Avatar { get; set; } = string.Empty;
		public IEnumerable<PhotoInfo>? Photos { get; set; }
	}
}
