namespace Traqtiv.API.Models.Entities
{
    public class DailyActivity : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public int Steps { get; set; }
        public int CaloriesBurned { get; set; }
        public int ActiveMinutes { get; set; }
        public double DistanceKm { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;
    }
}
