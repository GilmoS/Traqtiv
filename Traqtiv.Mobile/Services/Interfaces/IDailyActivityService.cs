namespace Traqtiv.Mobile.Services.Interfaces;

// Interface for managing daily activities, including retrieval and addition of activities and summaries.
public interface IDailyActivityService : IService
{
    // Retrieves a list of daily activities.
    Task<List<DailyActivityDto>> GetActivitiesAsync();

    // Retrieves a summary of activities within a specified date range.
    Task<ActivitySummaryDto?> GetActivitySummaryAsync(DateTime from, DateTime to);

    // Adds a new daily activity and returns a boolean indicating success or failure.
    Task<bool> AddDailyActivityAsync(AddDailyActivityDto request);
}