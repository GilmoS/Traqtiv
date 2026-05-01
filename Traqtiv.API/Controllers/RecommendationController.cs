using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traqtiv.API.Helpers;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Controllers
{

    // This controller handles user-specific recommendations and alerts
    // It requires authentication to access the endpoints
    [Authorize]
    public class RecommendationController : BaseController
    {
        private readonly IRecommendationService _recommendationService;

        // The constructor injects the recommendation service which contains the business logic for fetching recommendations and alerts
        // This promotes separation of concerns and makes the controller easier to test
        // The IRecommendationService is an interface that defines the contract for the recommendation service, allowing for different implementations if needed
        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }


        // This endpoint retrieves personalized recommendations for the authenticated user
        // It extracts the user ID from the JWT token and calls the service to get the recommendations
        // The result is returned in a standardized response format using the OkResponse helper method
        [HttpGet]
        public async Task<IActionResult> GetRecommendations()
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _recommendationService.GetRecommendationsAsync(userId);
            return OkResponse(result);
        }

        // This endpoint retrieves alerts for the authenticated user
        // Similar to the recommendations endpoint, it extracts the user ID from the JWT token and calls the service to get the alerts
        // The alerts are returned in a standardized response format using the OkResponse helper method
        [HttpGet("alerts")]
        public async Task<IActionResult> GetAlerts()
        {
            var userId = JwtHelper.GetUserIdFromToken(User);
            var result = await _recommendationService.GetAlertsAsync(userId);
            return OkResponse(result);
        }
        // This endpoint allows the user to mark a specific alert as read
        // It takes the alert ID as a parameter and calls the service to update the alert status
        // After marking the alert as read, it returns a success message in a standardized response format using the OkResponse helper method
        [HttpPut("alerts/{id}/read")]
        public async Task<IActionResult> MarkAlertAsRead(Guid id)
        {
            await _recommendationService.MarkAlertAsReadAsync(id);
            return OkResponse(new { Success = true, Message = "Alert marked as read" });
        }
    }
}
