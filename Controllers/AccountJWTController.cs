using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApiPOC.Data;
using WebApiPOC.OAuth.Interface;
using WebApiPOC.Services.IServices;

namespace WebApiPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly transactionDBContext _context;
        private readonly IAccountService _accountService;
        private readonly ITokenManager _tokenManager;

        // ============= Log Implementation =============================
        private readonly ILogger _logger;
        public AccountController(ILogger<AccountController> logger, transactionDBContext context, IAccountService accountService, ITokenManager tokenManager)
        {
            _context = context;
            _accountService = accountService;
            _tokenManager = tokenManager;
            _logger = logger;
        }
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthRequest authRequest)
        {
            try
            {
                if (authRequest == null || string.IsNullOrEmpty(authRequest.UserName) || string.IsNullOrEmpty(authRequest.Password)) return Unauthorized();

                var userObj = await _accountService.GetUserByUserNamePassword(authRequest.UserName, authRequest.Password);
                if (userObj == null) return Unauthorized();

                var authResult = _tokenManager.CreateToken(userObj.UserName);

                if (string.IsNullOrEmpty(authResult)) return Unauthorized();

                return Ok(authResult);
            }

            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
