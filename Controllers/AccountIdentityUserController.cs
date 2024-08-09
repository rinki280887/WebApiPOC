using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApiPOC.Data;
using WebApiPOC.DataBaseModel;

[Route("api/[controller]")]
[ApiController]
public class AccountIdentityUserController : ControllerBase
{
    //using IdentityUser for user management and IdentityRole for role management.
    //You can now manage user authentication, handle roles, and perform various identity-related operations in your application.
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountIdentityUserController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [HttpPost("Register User")]
    public async Task<IActionResult> CreateUser(RegisterModel regModel)
    {
        var identityUser = await _userManager.FindByNameAsync(regModel.UserName);
        if (identityUser != null) return Ok ("User Exist");

        var userObj = new User { UserName = regModel.UserName, Email = regModel.EmailAddress, 
            Password= regModel.Password, AddressName = regModel.AddressName, SecurityStamp = new Guid().ToString() };

        var result = await _userManager.CreateAsync(userObj, regModel.Password);

        if (result.Succeeded)
        {
            return Ok("User Created");
        }
        else
        {
            return BadRequest();
        }
    }

    private IActionResult StatusCode()
    {
        throw new NotImplementedException();
    }


    // Example: Login User
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _signInManager.PasswordSignInAsync(model.EmailAddress, model.Password, model.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return Ok("Login successful");
        }

        if (result.IsLockedOut)
        {
            return Unauthorized("User is locked out");
        }

        return Unauthorized("Invalid login attempt");
    }
}
