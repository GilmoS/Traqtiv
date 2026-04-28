using Traqtiv.API.Models.DTOs.Responses;

namespace Traqtiv.API.Services.Interfaces
{
 
    /// Provides operations to retrieve weather information for geographic coordinates.
    public interface IWeatherService
    {
        /// Retrieves current weather information for the specified location.
        Task<WeatherResponseDto> GetCurrentWeatherAsync(double latitude, double longitude);
    }
}