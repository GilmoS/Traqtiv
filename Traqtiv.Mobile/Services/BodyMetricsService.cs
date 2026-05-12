using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.Services;

// This service is responsible for handling body metrics related operations, such as fetching body metrics and adding new metrics.
public class BodyMetricsService : IBodyMetricsService
{
    private readonly ApiClient _apiClient;

    // Constructor that initializes the BodyMetricsService with an instance of ApiClient.
    public BodyMetricsService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Initializes the service. In this case, it simply returns a completed task, but it can be extended to perform any necessary setup.
    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }


    // Fetches the list of body metrics from the API.
    public async Task<List<BodyMetricsDto>> GetMetricsAsync()
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            var result = await _apiClient.Client.MetricsAllAsync();
            return result?.ToList() ?? new List<BodyMetricsDto>();
        }
        catch (Exception)
        {
            return new List<BodyMetricsDto>();
        }
    }


    // Adds new body metrics by sending the request to the API.
    public async Task<bool> AddMetricsAsync(AddMetricsDto request)
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            await _apiClient.Client.MetricsPOSTAsync(request);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}