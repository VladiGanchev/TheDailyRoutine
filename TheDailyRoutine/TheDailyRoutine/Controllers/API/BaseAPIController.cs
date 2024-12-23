using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheDailyRoutine.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public abstract class BaseAPIController : ControllerBase
    {
        protected IActionResult Success(object data = null, string message = null)
        {
            return Ok(new
            {
                success = true,
                message,
                data
            });
        }

        protected IActionResult Error(string message, int statusCode = 400)
        {
            return StatusCode(statusCode, new
            {
                success = false,
                message
            });
        }

        protected IActionResult NotFoundError(string message = "Resource not found")
        {
            return Error(message, 404);
        }

        protected IActionResult ValidationError(string message = "Validation failed", object errors = null)
        {
            return BadRequest(new
            {
                success = false,
                message,
                errors
            });
        }
    }
}