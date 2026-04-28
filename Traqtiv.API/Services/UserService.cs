using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;
using Traqtiv.API.Models.Entities;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Services
{
    /// Provides operations for retrieving and updating user profiles and body metrics.
    /// This service delegates data access to ISmartFitnessDal and maps entity models to DTOs.
    public class UserService : IUserService
    {
        private readonly ISmartFitnessDal _dal;

        /// Initializes a new instance of the UserServiceclass.
        /// The data access layer is used to query and persist user and metrics data.
        public UserService(ISmartFitnessDal dal)
        {
            _dal = dal;
        }

        /// Retrieves the profile for the specified user.
        /// Returns a UserProfileDto containing profile data. If the user is not found the returned DTO
        /// will have UserProfileDto.Success set to false and an explanatory UserProfileDto.Message.
        public async Task<UserProfileDto> GetProfileAsync(Guid userId)
        {
            var user = await _dal.GetUserByIdAsync(userId);
            if (user == null)
                return new UserProfileDto { Success = false, Message = "User not found" };

            return new UserProfileDto
            {
                Success = true,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth
            };
        }

        /// Updates the profile for the specified user.
        /// Returns a UserProfileDto containing the updated profile. If the user is not found the returned DTO
        /// will have UserProfileDto.Success set to falseand an explanatory UserProfileDto.Message.
        public async Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto request)
        {
            var user = await _dal.GetUserByIdAsync(userId);
            if (user == null)
                return new UserProfileDto { Success = false, Message = "User not found" };

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.DateOfBirth = request.DateOfBirth;

            await _dal.UpdateUserAsync(user);

            return new UserProfileDto
            {
                Success = true,
                Message = "Profile updated successfully",
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth
            };
        }


        /// Retrieves historical body metrics for the specified user.
        /// Returns a list of BodyMetricsDto containing recorded metrics.
        /// Each DTO's BodyMetricsDto.Success will be true for returned entries
        public async Task<List<BodyMetricsDto>> GetBodyMetricsAsync(Guid userId)
        {
            var metrics = await _dal.GetMetricsByUserIdAsync(userId);

            return metrics.Select(m => new BodyMetricsDto
            {
                Success = true,
                Weight = m.Weight,
                RestingHeartRate = m.RestingHeartRate,
                BMI = m.BMI,
                MeasuredAt = m.MeasuredAt
            }).ToList();
        }

        
        /// Adds a new body metrics entry for the specified user.
        /// The measurement timestamp is set to DateTime.UtcNow when the entry is created.
        public async Task AddBodyMetricsAsync(Guid userId, AddMetricsDto request)
        {
            var metrics = new BodyMetrics
            {
                UserId = userId,
                Weight = request.Weight,
                RestingHeartRate = request.RestingHeartRate,
                BMI = request.BMI,
                MeasuredAt = DateTime.UtcNow
            };

            await _dal.AddMetricsAsync(metrics);
        }
    }
}
