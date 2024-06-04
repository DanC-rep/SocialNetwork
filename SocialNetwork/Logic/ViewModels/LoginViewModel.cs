using System.ComponentModel.DataAnnotations;

namespace Logic.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; } = string.Empty;

		[Display(Name = "Запомнить меня")]
		public bool RememberMe { get; set; }
	}
}
