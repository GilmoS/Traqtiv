namespace Traqtiv.API.Models.DTOs.Responses
{
    public class DailyActivityDto : BaseResponseDto
    {
        public int Steps { get; set; }
        public int CaloriesBurned { get; set; }
        public int ActiveMinutes { get; set; }
        public double DistanceKm { get; set; }
        public DateTime Date { get; set; }
    }
}
