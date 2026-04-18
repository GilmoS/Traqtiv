namespace Traqtiv.API.Models.DTOs.Responses
{
    public class WeatherResponseDto : BaseResponseDto
    {
        public double Temperature { get; set; }
        public string Description { get; set; } = string.Empty;
        public int AirQualityIndex { get; set; }
        public string AirQualityDescription { get; set; } = string.Empty;
    }
}
