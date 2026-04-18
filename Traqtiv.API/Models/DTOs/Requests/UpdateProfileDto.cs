namespace Traqtiv.API.Models.DTOs.Requests
{
    public class UpdateProfileDto : BaseRequestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
