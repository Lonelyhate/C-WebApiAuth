using System.ComponentModel.DataAnnotations;
using DataType = Swashbuckle.AspNetCore.SwaggerGen.DataType;

namespace apiLeran.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите эмаил")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Введите имя")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Введите пароль")]

    public string Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]

    [Required(ErrorMessage = "Подтвердите пароль")]
    public string PasswordConfrim { get; set; }
}