using apiLeran.Data.Repositories;
using apiLeran.Interfaces;
using apiLeran.Models;
using apiLeran.Services;
using apiLeran.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace apiLeran.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly UserRepository _userRepository;
    private readonly IMessageEmailService _messageEmail;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
        UserRepository userRepository, IMessageEmailService messageEmail)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _messageEmail = messageEmail;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        if (User.Identity.IsAuthenticated)
        {
            User user = await _userRepository.GetById(id);

            if (user is not null) return new ObjectResult(user);

            return NotFound("Пользователь не найден");
        }

        return Content("Необходимо зарегистрироваться");
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        if (User.Identity.IsAuthenticated)
        {
            var users = await _userRepository.GetUsers();

            if (users is not null) return new ObjectResult(users);

            return NotFound("Пользователей нет");
        }

        return Content("Необходимо зарегистрироваться");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var _user = await _userRepository.GetByName(model.Name);
        if (_user == null)
        {
            
            if (model.Password == model.PasswordConfrim)
            {
                User user = new User { Email = model.Email, UserName = model.Name, Name = model.Name };

                try
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                            protocol: HttpContext.Request.Scheme);
                        EmailService emailService = new EmailService();
                        await _messageEmail.SendMessage(model.Email, "Плдтврждение аккаунта",
                            $"Подтвердите регистрацию, перейдя по ссылку: <a href='{callbackUrl}'>link</a>");
                        IdentityRole getRoleId = await _userRepository.GetRole("user");

                        var userRole = new IdentityUserRole<string>() { UserId = user.Id, RoleId = getRoleId.Id };

                        await _userRepository.AddRole(userRole);
                        await _userRepository.Save();

                        return Ok("Регистрация успещна пройдена");
                    }
                }
                catch (Exception e)
                {
                    await _userRepository.Delete(user.Id);
                    await _userRepository.Save();

                    return NotFound("Почта не найдена");
                }
            }
            else
            {
                {
                    return Content("Пароли не совпадают");
                }
            }
        }
        else
        {
            return Content("Пользователь с таким ником уже есть");
        }
        return Content("хз что");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userRepository.GetByEmail(model.Email);

            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return Content($"Email пользователя {model.Email} не подтвержден");
                }
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                return Ok("Успешно");
            }
            else
            {
                return Content("Неправильный логин или пароль");
            }
        }

        return ValidationProblem("Ошибка при наборе данных в форме");
    }

    [HttpPost("logoff")]
    public async Task<IActionResult> LogOff()
    {
        var userEmail = User.Identity.Name;
        var user = await _userRepository.GetByEmail(userEmail);

        await _signInManager.SignOutAsync();

        return Ok("Выход успешно прошел");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfrimEmail(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return Content("Error");
        }
        
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return Content("Error");

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            return Ok("Почта подтверждена");
        }
        else
        {
            return Content("Почта не подтверждена");
        }
    }
}