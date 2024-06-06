﻿using System.ComponentModel.DataAnnotations;

namespace Logic.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Введите Email")]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Введите пароль")]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; } = string.Empty;

		[Display(Name = "Запомнить меня")]
		public bool RememberMe { get; set; }
	}
}
