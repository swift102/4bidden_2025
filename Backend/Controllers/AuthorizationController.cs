using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Backend.Service;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService _emailService;

    public AuthorizationController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            Name = model.Name
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _emailService.SendWelcomeEmail(model.Email, model.Password);
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid login attempt.");
        }

        return Ok("Login successful.");
    }
}

