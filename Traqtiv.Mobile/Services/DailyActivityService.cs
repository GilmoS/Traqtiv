using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.Services;

public class DailyActivityService : IDailyActivityService
{
    private readonly ApiClient _apiClient;

    public DailyActivityService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

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