using Logic.Enums;
using System.ComponentModel.DataAnnotations;

namespace Logic.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
		public string ConfirmPassword { get; set; } = string.Empty;

		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Surname { get; set; } = string.Empty;

		public string? Patronymic { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime BirthDate { get; set; }

		[Required]
		public string City { get; set; } = string.Empty;

		[Required]
		public string Country { get; set; } = string.Empty;

		[Required]
		public Gender Gender = Gender.Secret;
	}
}
