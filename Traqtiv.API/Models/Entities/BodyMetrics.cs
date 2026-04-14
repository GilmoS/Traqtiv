namespace Traqtiv.API.Models.Entities
{
    public class BodyMetrics : BaseEntity
    {
        public Guid UserId { get; set; }
        public double Weight { get; set; }
        public int RestingHeartRate { get; set; }
        public double BMI { get; set; }
        public DateTime MeasuredAt { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;
    }
}