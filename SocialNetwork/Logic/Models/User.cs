using Logic.Enums;
using Microsoft.AspNetCore.Identity;

namespace Logic.Models
{
	public class User : IdentityUser
	{
		public string Name { get; set; } = string.Empty;
		public string Surname { get; set; } = string.Empty;
		public string? Patronymic { get; set; } = string.Empty;

		public DateTime BirthDate { get; set; }

		public string City { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;

		public Gender Gender { get; set; } = Gender.Secret;
	}
}
