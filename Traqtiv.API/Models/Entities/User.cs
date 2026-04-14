using Traqtiv.API.Models.Enums;

namespace Traqtiv.API.Models.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        // Navigation Properties
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
        public ICollection<BodyMetrics> BodyMetrics { get; set; } = new List<BodyMetrics>();
        public ICollection<DailyActivity> DailyActivities { get; set; } = new List<DailyActivity>();
        public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
        public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    }
}