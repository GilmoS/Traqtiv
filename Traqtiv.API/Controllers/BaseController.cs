using Microsoft.AspNetCore.Mvc;

namespace Traqtiv.API.Controllers
{
    // BaseController provides common response methods for all controllers in the API
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        // OkResponse returns a standardized success response with the provided data
        protected IActionResult OkResponse<T>(T data)
        {
            return Ok(data);
        }
        // ErrorResponse returns a standardized error response with the provided message
        protected IActionResult ErrorResponse(string message)
        {
            return BadRequest(new { Success = false, Message = message });
        }
        // NotFoundResponse returns a standardized not found response with the provided message
        protected IActionResult NotFoundResponse(string message)
        {
            return NotFound(new { Success = false, Message = message });
        }
    }
}