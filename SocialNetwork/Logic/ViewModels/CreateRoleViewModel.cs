using System.ComponentModel.DataAnnotations;

namespace Logic.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Введите название роли")]
        [Display(Name = "Роль")]
        public string Role { get; set; } = string.Empty;
    }
}
