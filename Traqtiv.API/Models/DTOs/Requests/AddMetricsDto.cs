namespace Traqtiv.API.Models.DTOs.Requests
{
    public class AddMetricsDto : BaseRequestDto
    {
        public double Weight { get; set; }
        public int RestingHeartRate { get; set; }
        public double BMI { get; set; }
    }
}