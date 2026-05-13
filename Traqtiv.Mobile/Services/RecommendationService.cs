using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.Services;

// This service is responsible for handling recommendation-related operations, such as fetching recommendations and alerts from the API, and marking alerts as read.
// It uses an instance of ApiClient to communicate with the backend API and perform these operations.
// The service includes methods to initialize itself, retrieve recommendations and alerts, and mark specific alerts as read.
public class RecommendationService : IRecommendationService
{
    private readonly ApiClient _apiClient;

    // Constructor that initializes the service with an instance of ApiClient, which is used to communicate with the backend API.
    public RecommendationService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }


    // Initializes the service. In this case, it simply returns a completed task, but it can be extended to perform any necessary setup.
    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }


    // Fetches a list of recommendations from the API. It first attaches the authentication token to the API client, then calls the RecommendationsAllAsync method to retrieve the data.
    public async Task<List<RecommendationDto>> GetRecommendationsAsync()
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            var result = await _apiClient.Client.RecommendationsAllAsync();
            return result?.ToList() ?? new List<RecommendationDto>();
        }
        catch (Exception)
        {
            return new List<RecommendationDto>();
        }
    }


    // Fetches a list of alerts from the API. Similar to fetching recommendations, it attaches the authentication token and calls the AlertsAsync method to retrieve the data.
    public async Task<List<AlertDto>> GetAlertsAsync()
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            var result = await _apiClient.Client.AlertsAsync();
            return result?.ToList() ?? new List<AlertDto>();
        }
        catch (Exception)
        {
            return new List<AlertDto>();
        }
    }


    // Marks a specific alert as read by its ID.
    // It attaches the authentication token and calls the AlertsAsync method with the alert ID to perform the action.
    public async Task<bool> MarkAlertAsReadAsync(Guid id)
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            await _apiClient.Client.AlertsAsync(id);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}