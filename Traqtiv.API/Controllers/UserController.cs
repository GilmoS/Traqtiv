using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traqtiv.API.Helpers;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Controllers
{
    // UserController handles user-related endpoints such as profile management and body metrics
    // It inherits from BaseController to utilize common response methods and is protected by the Authorize attribute to ensure that only authenticated users can access its endpoints
    // The controller uses the IUserService to perform operations related to user profiles and body metrics
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        // Constructor for UserController that injects the IUserService dependency
        // This allows the controller to use user services for handling profile management and body metrics logic
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // GET api/user/profile endpoint for retrieving the user's profile information
        // It extracts the user ID from the JWT token using the JwtHelper and calls the GetProfileAsync method of the IUserService
        // The method returns an appropriate response based on the result of the service call
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _userService.GetProfileAsync(userId);
            if (!result.Success)
                return NotFoundResponse(result.Message);

            return OkResponse(result);
        }


        // PUT api/user/profile endpoint for updating the user's profile information
        // It accepts an UpdateProfileDto object in the request body, extracts the user ID from the JWT token, and calls the UpdateProfileAsync method of the IUserService
        // The method returns an appropriate response based on the result of the service call
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto request)
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _userService.UpdateProfileAsync(userId, request);
            if (!result.Success)
                return NotFoundResponse(result.Message);

            return OkResponse(result);
        }


        // GET api/user/metrics endpoint for retrieving the user's body metrics
        // It extracts the user ID from the JWT token and calls the GetBodyMetricsAsync method of the IUserService
        // The method returns an appropriate response based on the result of the service call
        [HttpGet("metrics")]
        public async Task<IActionResult> GetBodyMetrics()
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _userService.GetBodyMetricsAsync(userId);
            return OkResponse(result);
        }


        // POST api/user/metrics endpoint for adding new body metrics for the user
        // It accepts an AddMetricsDto object in the request body, extracts the user ID from the JWT token, and calls the AddBodyMetricsAsync method of the IUserService
        // The method returns a success response if the metrics were added successfully
        [HttpPost("metrics")]
        public async Task<IActionResult> AddBodyMetrics([FromBody] AddMetricsDto request)
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            await _userService.AddBodyMetricsAsync(userId, request);
            return OkResponse(new { Success = true, Message = "Metrics added successfully" });
        }
    }
}