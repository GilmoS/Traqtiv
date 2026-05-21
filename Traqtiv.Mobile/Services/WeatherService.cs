using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.Services;

//This service is responsible for fetching current weather data based on geographic coordinates.
//It uses the generated ApiClient to call the WeatherAsync method, which interacts with the backend API to retrieve weather information.
//The service handles any exceptions that may occur during the API call and returns null if the data cannot be fetched successfully.
public class WeatherService : IWeatherService
{
    private readonly ApiClient _apiClient;

    public WeatherService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    // Retrieves the current weather data for the specified latitude and longitude.
    // It calls the WeatherAsync method of the ApiClient, which interacts with the backend API to fetch weather information.
    // If an exception occurs during the API call, the method returns null.
    public async Task<WeatherResponseDto?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            return await _apiClient.Client.WeatherAsync(latitude, longitude);
        }
        catch (Exception)
        {
            return null;
        }
    }
}