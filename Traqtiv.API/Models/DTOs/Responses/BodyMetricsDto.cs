namespace Traqtiv.API.Models.DTOs.Responses
{
    public class BodyMetricsDto : BaseResponseDto
    {
        public double Weight { get; set; }
        public int RestingHeartRate { get; set; }
        public double BMI { get; set; }
        public DateTime MeasuredAt { get; set; }
    }
}
