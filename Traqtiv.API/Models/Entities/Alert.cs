using Traqtiv.API.Models.Enums;

namespace Traqtiv.API.Models.Entities
{
    public class Alert : BaseEntity
    {
        public Guid UserId { get; set; }
        public AlertType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public AlertSeverity Severity { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;
    }
}