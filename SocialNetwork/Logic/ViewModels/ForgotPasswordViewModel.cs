using System.ComponentModel.DataAnnotations;

namespace Logic.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}