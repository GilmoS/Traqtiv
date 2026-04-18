using Traqtiv.API.Models.Enums;

namespace Traqtiv.API.Models.DTOs.Responses
{
    public class AlertDto : BaseResponseDto
    {
        public Guid Id { get; set; }
        public AlertType Type { get; set; }
        public bool IsRead { get; set; }
        public AlertSeverity Severity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
