using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiPOC.Data;

namespace WebApiPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountRoleController : Controller
    {
        public class RoleController : ControllerBase
        {
            //using IdentityUser for user management and IdentityRole for role management.
            //You can now manage user authentication, handle roles, and perform various identity-related operations in your application.
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly UserManager<IdentityUser> _userManager;

            public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
            {
                _roleManager = roleManager;
                _userManager = userManager;
            }

            [HttpPost("create-role")]
            public async Task<IActionResult> CreateRole([FromBody] string roleName)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (roleExists)
                    return BadRequest("Role already exists");

                var role = new IdentityRole(roleName);
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return Ok("Role created successfully");
                }

                return BadRequest(result.Errors);
            }

            [HttpPost("assign-role")]
            public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (user == null)
                    return BadRequest("User not found");

                var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                if (result.Succeeded)
                {
                    return Ok("Role assigned successfully");
                }

                return BadRequest(result.Errors);
            }
        }

    }
}

