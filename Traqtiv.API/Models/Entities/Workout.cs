using Traqtiv.API.Models.Enums;

namespace Traqtiv.API.Models.Entities
{
    public class Workout : BaseEntity
    {
        public Guid UserId { get; set; }
        public WorkoutType Type { get; set; }
        public int DurationMinutes { get; set; }
        public WorkoutStatus Status { get; set; }
        public int CaloriesBurned { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; } = string.Empty;

        // Navigation Property
        public User User { get; set; } = null!;
    }
}
