﻿using Logic.DataAnnotations;
using Logic.Enums;
using System.ComponentModel.DataAnnotations;

namespace Logic.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Введите Email")]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Введите пароль")]
		[Display(Name = "Пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "Необходимо подтверждение пароля")]
		[DataType(DataType.Password)]
		[Display(Name = "Подтвердите пароль")]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		public string ConfirmPassword { get; set; } = string.Empty;

		[Required(ErrorMessage = "Введите имя")]
		[Display(Name = "Имя")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Введите фамилию")]
		[Display(Name = "Фамилия")]
		public string Surname { get; set; } = string.Empty;

		[Display(Name = "Отчество (если есть)")]
		public string? Patronymic { get; set; }

		[Required(ErrorMessage = "Введите дату рождения")]
		[DataType(DataType.Date)]
		[ValidUserAge(_minAge: 14)]
		[Display(Name = "Дата рождения")]
		public DateTime? BirthDate { get; set; }

		[Required(ErrorMessage = "Введите город")]
		[Display(Name = "Город")]
		public string City { get; set; } = string.Empty;

		[Required(ErrorMessage = "Введите страну")]
		[Display(Name = "Страна")]
		public string Country { get; set; } = string.Empty;

		[Required(ErrorMessage = "Введите пол")]
		[Display(Name = "Пол")]
		public Gender Gender { get; set; } = Gender.Secret;
	}
}
