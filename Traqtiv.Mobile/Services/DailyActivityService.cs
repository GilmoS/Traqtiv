using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.Services;


//This service is responsible for handling daily activity related operations, such as fetching activities, getting activity summaries, and adding new activities.
//It interacts with the API client to perform these operations and handles any exceptions that may occur during the process.
public class DailyActivityService : IDailyActivityService
{
    private readonly ApiClient _apiClient;

    // Constructor that initializes the DailyActivityService with an instance of ApiClient.
    public DailyActivityService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Initializes the service. In this case, it simply returns a completed task, but it can be extended to perform any necessary setup.
    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    // Fetches the list of daily activities from the API.
    // It attaches the token for authentication and handles any exceptions that may occur, returning an empty list if an error occurs.
    public async Task<List<DailyActivityDto>> GetActivitiesAsync()
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            var result = await _apiClient.Client.ActivitiesAllAsync();
            return result?.ToList() ?? new List<DailyActivityDto>();
        }
        catch (Exception)
        {
            return new List<DailyActivityDto>();
        }
    }

    // Fetches the activity summary for a given date range from the API.
    public async Task<ActivitySummaryDto?> GetActivitySummaryAsync(DateTime from, DateTime to)
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            return await _apiClient.Client.SummaryAsync(from, to);
        }
        catch (Exception)
        {
            return null;
        }
    }

    // Adds a new daily activity by sending the request to the API.
    public async Task<bool> AddDailyActivityAsync(AddDailyActivityDto request)
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            await _apiClient.Client.ActivitiesPOSTAsync(request);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}