namespace Traqtiv.API.Models.DTOs.Requests
{
    public class AddDailyActivityDto : BaseRequestDto
    {
        public int Steps { get; set; }
        public int CaloriesBurned { get; set; }
        public int ActiveMinutes { get; set; }
        public double DistanceKm { get; set; }
        public DateTime Date { get; set; }
    }
}
