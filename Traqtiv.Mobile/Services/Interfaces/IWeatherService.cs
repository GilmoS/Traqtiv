using Traqtiv.Mobile.Models;

namespace Traqtiv.Mobile.Services.Interfaces;

// Defines the contract for a weather service that retrieves current weather and air quality data based on geographic coordinates.

public interface IWeatherService : IService
{
    // Retrieves the current weather and air quality data for the specified geographic coordinates.
    Task<WeatherResponseDto?> GetCurrentWeatherAsync(double latitude, double longitude);
}