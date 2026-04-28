using Traqtiv.API.Models.DTOs.Responses;

namespace Traqtiv.API.Services.Interfaces
{
    /// Provides recommendation- and alert-related operations for a user.

    public interface IRecommendationService
    {
        
        /// Retrieves personalized recommendations for the specified user.
        Task<List<RecommendationDto>> GetRecommendationsAsync(Guid userId);

        
        /// Retrieves active alerts for the specified user.
        Task<List<AlertDto>> GetAlertsAsync(Guid userId);


        /// Marks the specified alert as read.
        Task MarkAlertAsReadAsync(Guid alertId);

        
        /// Notifies the recommendation service that a workout was added for the given user.
        Task OnWorkoutAddedAsync(Guid userId);

        
        /// Notifies the recommendation service that a user's daily activity record was updated.
        Task OnDailyActivityUpdatedAsync(Guid userId);
    }
}