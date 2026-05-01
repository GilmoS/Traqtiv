using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traqtiv.API.Helpers;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Controllers
{
    // DailyActivityController handles endpoints related to daily activities such as retrieving, adding, and summarizing daily activities
    // It inherits from BaseController to utilize common response methods and is protected by the Authorize attribute to ensure that only authenticated users can access its endpoints
    // The controller uses the IDailyActivityService to perform operations related to daily activities
    [Authorize]
    public class DailyActivityController : BaseController
    {
        private readonly IDailyActivityService _activityService;


        // Constructor for DailyActivityController that injects the IDailyActivityService dependency
        // This allows the controller to use daily activity services for handling daily activity-related logic
        // The constructor initializes the _activityService field with the provided IDailyActivityService instance, enabling the controller to call methods for managing daily activities
        public DailyActivityController(IDailyActivityService activityService)
        {
            _activityService = activityService;
        }


        // GET api/dailyactivity endpoint for retrieving all daily activities for the authenticated user
        // It extracts the user ID from the JWT token using the JwtHelper and calls the GetActivitiesAsync method of the IDailyActivityService
        // The method returns an appropriate response based on the result of the service call
        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _activityService.GetActivitiesAsync(userId);
            return OkResponse(result);
        }
        // POST api/dailyactivity endpoint for adding a new daily activity for the authenticated user
        // It accepts an AddDailyActivityDto object in the request body, extracts the user ID from the JWT token, and calls the AddDailyActivityAsync method of the IDailyActivityService
        // The method returns an appropriate response based on the result of the service call
        [HttpPost]
        public async Task<IActionResult> AddDailyActivity([FromBody] AddDailyActivityDto request)
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            await _activityService.AddDailyActivityAsync(userId, request);
            return OkResponse(new { Success = true, Message = "Activity added successfully" });
        }


        // GET api/dailyactivity/summary endpoint for retrieving a summary of daily activities for the authenticated user within a specified date range
        // It accepts 'from' and 'to' date parameters in the query string, extracts the user ID from the JWT token, and calls the GetActivitySummaryAsync method of the IDailyActivityService
        // The method returns an appropriate response based on the result of the service call, providing a summary of activities for the specified date range
        [HttpGet("summary")]
        public async Task<IActionResult> GetActivitySummary([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _activityService.GetActivitySummaryAsync(userId, from, to);
            return OkResponse(result);
        }
    }
}