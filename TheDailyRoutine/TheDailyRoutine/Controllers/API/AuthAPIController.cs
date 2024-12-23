using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TheDailyRoutine.Infrastructure.Data.Models;

namespace TheDailyRoutine.Controllers.API
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : BaseAPIController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthAPIController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(
                request.Username,
                request.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                return Ok(new
                {
                    token = "dummy-token",
                    username = user.UserName
                });
            }

            return BadRequest(new { error = "Invalid username or password" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}