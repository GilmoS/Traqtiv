using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;

namespace Traqtiv.API.Services.Interfaces
{

    /// Defines operations for managing a user's workouts, including retrieval, creation, update and deletion.
    public interface IWorkoutService
    {

        /// Retrieves all workouts for the specified user.
        Task<List<WorkoutDto>> GetWorkoutsAsync(Guid userId);

        
        /// Retrieves a single workout by id for the specified user.
        Task<WorkoutDto> GetWorkoutByIdAsync(Guid userId, Guid workoutId);

        
        /// Adds a new workout for the specified user.
        Task<WorkoutDto> AddWorkoutAsync(Guid userId, AddWorkoutDto request);

        
        /// Updates an existing workout for the specified user.
        Task<WorkoutDto> UpdateWorkoutAsync(Guid userId, Guid workoutId, UpdateWorkoutDto request);

        
        /// Deletes the specified workout for the given user.
        Task DeleteWorkoutAsync(Guid userId, Guid workoutId);
    }
}