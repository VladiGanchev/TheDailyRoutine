using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TheDailyRoutine.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using TheDailyRoutine.Models;

namespace TheDailyRoutine.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthAPIController : BaseAPIController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthAPIController> _logger;
        public AuthAPIController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<AuthAPIController> logger)

        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ErrorResponse { Error = "Invalid request" });
            }

            var user = new ApplicationUser { UserName = request.Username, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new AuthResponse
                {
                    Token = "dummy-token",
                    Username = user.UserName
                });
            }

            return BadRequest(new ErrorResponse
            {
                Error = string.Join(", ", result.Errors.Select(e => e.Description))
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ErrorResponse { Error = "Invalid request" });
            }

            // First try to find user by username
            var user = await _userManager.FindByNameAsync(request.Identifier);

            // If not found by username, try email
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(request.Identifier);
            }

            if (user == null)
            {
                return BadRequest(new ErrorResponse { Error = "Invalid username/email or password" });
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName, // Use the username for sign in
                request.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new AuthResponse
                {
                    Token = "dummy-token",
                    Username = user.UserName
                });
            }

            return BadRequest(new ErrorResponse { Error = "Invalid username/email or password" });
        }
    }

    public class ErrorResponse
    {
        public string Error { get; set; }
    }
}