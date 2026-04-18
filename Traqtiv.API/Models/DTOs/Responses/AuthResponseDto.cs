namespace Traqtiv.API.Models.DTOs.Responses
{
    public class AuthResponseDto : BaseResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
