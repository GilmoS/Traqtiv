using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;

namespace Traqtiv.API.Services.Interfaces
{
    /// Defines operations for managing and querying a user's daily activities.
    public interface IDailyActivityService
    {
        /// Retrieves all daily activities for the specified user.
        Task<List<DailyActivityDto>> GetActivitiesAsync(Guid userId);

        /// Adds a new daily activity for the specified user.
        Task AddDailyActivityAsync(Guid userId, AddDailyActivityDto request);

        /// Gets an aggregated summary of a user's activities for the specified date range.
        Task<ActivitySummaryDto> GetActivitySummaryAsync(Guid userId, DateTime from, DateTime to);
    }
}
