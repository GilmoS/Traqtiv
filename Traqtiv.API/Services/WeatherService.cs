using Traqtiv.API.Models.DTOs.Responses;
using Traqtiv.API.Services.Interfaces;
using System.Text.Json;

namespace Traqtiv.API.Services
{
    /// Provides operations to retrieve current weather and air quality data based on geographic coordinates.
    /// This service uses the OpenWeatherMap API to fetch weather and air pollution data.
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        /// Initializes a new instance of the WeatherService class.
        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        /// Retrieves the current weather and air quality data for the specified latitude and longitude.
        /// Returns a WeatherResponseDto containing the temperature, weather description, air quality index and description.
        /// If the API call fails, the returned DTO will have Success set to false and an explanatory Message.
        public async Task<WeatherResponseDto> GetCurrentWeatherAsync(double latitude, double longitude)
        {
            // OpenWeatherMap Current Weather API: https://openweathermap.org/current
            try
            {
                var apiKey = _configuration["WeatherApi:ApiKey"];
                var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return new WeatherResponseDto { Success = false, Message = "Failed to get weather data" };

                var json = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<JsonElement>(json);

                var temperature = weatherData.GetProperty("main").GetProperty("temp").GetDouble();
                var description = weatherData.GetProperty("weather")[0].GetProperty("description").GetString() ?? "";

                // Get air quality data
                // OpenWeatherMap Air Pollution API: https://openweathermap.org/api/air-pollution
                var airUrl = $"http://api.openweathermap.org/data/2.5/air_pollution?lat={latitude}&lon={longitude}&appid={apiKey}";
                var airResponse = await _httpClient.GetAsync(airUrl);
                var airQualityIndex = 1;
                var airQualityDescription = "Good";

                /// Air quality index: 1=Good, 2=Fair, 3=Moderate, 4=Poor, 5=Very Poor
                /// We convert this to a human-readable description using GetAirQualityDescription method.
                if (airResponse.IsSuccessStatusCode)
                {
                    var airJson = await airResponse.Content.ReadAsStringAsync();
                    var airData = JsonSerializer.Deserialize<JsonElement>(airJson);
                    airQualityIndex = airData.GetProperty("list")[0].GetProperty("main").GetProperty("aqi").GetInt32();
                    airQualityDescription = GetAirQualityDescription(airQualityIndex);
                }

                return new WeatherResponseDto
                {
                    Success = true,
                    Temperature = temperature,
                    Description = description,
                    AirQualityIndex = airQualityIndex,
                    AirQualityDescription = airQualityDescription
                };
            }
            catch
            {
                return new WeatherResponseDto { Success = false, Message = "Failed to get weather data" };
            }
        }
        /// Converts the air quality index to a human-readable description.
        private static string GetAirQualityDescription(int aqi) => aqi switch
        {
            1 => "Good",
            2 => "Fair",
            3 => "Moderate",
            4 => "Poor",
            5 => "Very Poor",
            _ => "Unknown"
        };
    }
}
