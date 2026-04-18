namespace Traqtiv.API.Models.DTOs.Responses
{
    public abstract class BaseResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
