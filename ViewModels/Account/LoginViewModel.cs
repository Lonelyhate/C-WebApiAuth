using System.ComponentModel.DataAnnotations;

namespace apiLeran.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите Email")]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Введите пароль")]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
}