using Microsoft.AspNetCore.Mvc;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Controllers
{
    // AuthController handles user authentication-related endpoints such as registration and login
    // It inherits from BaseController to utilize common response methods
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        // Constructor for AuthController that injects the IAuthService dependency
        // This allows the controller to use authentication services for handling registration and login logic
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        // POST api/auth/register endpoint for user registration
        // It accepts a RegisterRequestDto object in the request body and returns an IActionResult
        // The method calls the RegisterAsync method of the IAuthService and returns an appropriate response based on the result
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.Success)
                return ErrorResponse(result.Message);

            return OkResponse(result);
        }
        
        // POST api/auth/login endpoint for user login
        // It accepts a LoginRequestDto object in the request body and returns an IActionResult
        // The method calls the LoginAsync method of the IAuthService and returns an appropriate response based on the result
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Success)
                return ErrorResponse(result.Message);

            return OkResponse(result);
        }
    }
}
