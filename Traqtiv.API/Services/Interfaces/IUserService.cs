using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;

namespace Traqtiv.API.Services.Interfaces
{
    /// Defines operations for retrieving and updating user profile and body metrics.
    public interface IUserService
    {
        /// Retrieves the profile information for the specified user.
        Task<UserProfileDto> GetProfileAsync(Guid userId);


        /// Updates the profile for the specified user with the provided values.
        Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto request);

        
        /// Retrieves historical body metrics for the specified user.
        Task<List<BodyMetricsDto>> GetBodyMetricsAsync(Guid userId);

        
        /// Adds a new body metrics entry for the specified user.
        Task AddBodyMetricsAsync(Guid userId, AddMetricsDto request);
    }
}
