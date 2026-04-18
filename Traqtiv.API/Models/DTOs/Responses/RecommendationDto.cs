using Traqtiv.API.Models.Enums;

namespace Traqtiv.API.Models.DTOs.Responses
{
    public class RecommendationDto : BaseResponseDto
    {
        public Guid Id { get; set; }
        public RecommendationType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}