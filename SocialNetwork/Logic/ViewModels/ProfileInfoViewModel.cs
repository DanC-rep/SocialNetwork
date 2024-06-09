using Logic.Enums;
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
	}
}
