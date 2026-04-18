namespace Traqtiv.API.Models.DTOs.Requests
{
    public class LoginRequestDto : BaseRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
