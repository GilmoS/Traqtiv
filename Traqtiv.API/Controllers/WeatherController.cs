using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Controllers
{
    // This controller handles weather-related endpoints, providing current weather information based on latitude and longitude
    // It requires authentication to access the endpoints, ensuring that only authorized users can retrieve weather data
    // The controller uses dependency injection to receive an instance of the IWeatherService, which contains the business logic for fetching weather data
    [Authorize]
    public class WeatherController : BaseController
    {
        private readonly IWeatherService _weatherService;

        // The constructor injects the weather service, allowing the controller to call methods that fetch weather data
        // This promotes separation of concerns and makes the controller easier to test, as the IWeatherService can be mocked in unit tests
        // The IWeatherService is an interface that defines the contract for the weather service, allowing for different implementations if needed (e.g., different weather APIs)
        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        // This endpoint retrieves the current weather information based on the provided latitude and longitude
        // It takes the latitude and longitude as query parameters and calls the weather service to get the current weather data
        // The result is returned in a standardized response format using the OkResponse helper method if successful, or an error response if the service call fails
        [HttpGet]
        public async Task<IActionResult> GetCurrentWeather([FromQuery] double latitude,[FromQuery] double longitude)
        {
            var result = await _weatherService.GetCurrentWeatherAsync(latitude, longitude);
            if (!result.Success)
                return ErrorResponse(result.Message);

            return OkResponse(result);
        }
    }
}