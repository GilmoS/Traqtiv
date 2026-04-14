using Traqtiv.API.Models.Enums;

namespace Traqtiv.API.Models.Entities
{
    public class Recommendation : BaseEntity
    {
        public Guid UserId { get; set; }
        public RecommendationType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;

        // Navigation Property
        public User User { get; set; } = null!;
    }
}
