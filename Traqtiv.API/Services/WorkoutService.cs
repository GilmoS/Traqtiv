using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;
using Traqtiv.API.Models.Entities;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Services
{

    /// Provides operations to create, retrieve, update and delete user workouts.
    /// This service maps entity models to DTOs and delegates persistence to ISmartFitnessDal
    /// It also notifies IRecommendationService when workouts are added.
    public class WorkoutService : IWorkoutService
    {
        private readonly ISmartFitnessDal _dal;
        private readonly IRecommendationService _recommendationService;

        /// Initializes a new instance of the WorkoutService class.
        /// The data access layer is used to query and persist workout data.
        /// The recommendation service is notified when workouts are added.
        public WorkoutService(ISmartFitnessDal dal, IRecommendationService recommendationService)
        {
            _dal = dal;
            _recommendationService = recommendationService;
        }

        /// Retrieves all workouts for the specified user.
        /// Returns a list of WorkoutDto objects. Each DTO has Success set to true. If the user has no workouts an empty list is returned.
        public async Task<List<WorkoutDto>> GetWorkoutsAsync(Guid userId)
        {
            var workouts = await _dal.GetWorkoutsByUserIdAsync(userId);

            return workouts.Select(w => new WorkoutDto
            {
                Success = true,
                Id = w.Id,
                Type = w.Type,
                DurationMinutes = w.DurationMinutes,
                Status = w.Status,
                CaloriesBurned = w.CaloriesBurned,
                Date = w.Date,
                Notes = w.Notes
            }).ToList();
        }
        /// Retrieves a specific workout by ID for the specified user.
        /// Returns a WorkoutDto containing the workout data. If the workout is not found or does not belong to the user, the returned DTO will have Success set to false and an explanatory Message.
        public async Task<WorkoutDto> GetWorkoutByIdAsync(Guid userId, Guid workoutId)
        {
            var workout = await _dal.GetWorkoutByIdAsync(workoutId);
            if (workout == null || workout.UserId != userId)
                return new WorkoutDto { Success = false, Message = "Workout not found" };

            return new WorkoutDto
            {
                Success = true,
                Id = workout.Id,
                Type = workout.Type,
                DurationMinutes = workout.DurationMinutes,
                Status = workout.Status,
                CaloriesBurned = workout.CaloriesBurned,
                Date = workout.Date,
                Notes = workout.Notes
            };
        }
        /// Adds a new workout for the specified user.
        /// Returns a WorkoutDto containing the created workout data. If creation is successful, Success is true and Message indicates success. The new workout's ID is included in the returned DTO.
        public async Task<WorkoutDto> AddWorkoutAsync(Guid userId, AddWorkoutDto request)
        {
            var workout = new Workout
            {
                UserId = userId,
                Type = request.Type,
                DurationMinutes = request.DurationMinutes,
                Status = request.Status,
                CaloriesBurned = request.CaloriesBurned,
                Date = request.Date,
                Notes = request.Notes
            };

            await _dal.AddWorkoutAsync(workout);

            /// (Observer) Notify RecommendationService about the new workout
            await _recommendationService.OnWorkoutAddedAsync(userId);

            return new WorkoutDto
            {
                Success = true,
                Message = "Workout added successfully",
                Id = workout.Id,
                Type = workout.Type,
                DurationMinutes = workout.DurationMinutes,
                Status = workout.Status,
                CaloriesBurned = workout.CaloriesBurned,
                Date = workout.Date,
                Notes = workout.Notes
            };
        }


        /// Updates an existing workout for the specified user.
        /// Returns a WorkoutDto containing the updated workout data. If the workout is not found or does not belong to the user, the returned DTO will have Success set to false and an explanatory Message.
        public async Task<WorkoutDto> UpdateWorkoutAsync(Guid userId, Guid workoutId, UpdateWorkoutDto request)
        {
            var workout = await _dal.GetWorkoutByIdAsync(workoutId);
            if (workout == null || workout.UserId != userId)
                return new WorkoutDto { Success = false, Message = "Workout not found" };

            workout.Type = request.Type;
            workout.DurationMinutes = request.DurationMinutes;
            workout.Status = request.Status;
            workout.CaloriesBurned = request.CaloriesBurned;
            workout.Date = request.Date;
            workout.Notes = request.Notes;

            await _dal.UpdateWorkoutAsync(workout);

            return new WorkoutDto
            {
                Success = true,
                Message = "Workout updated successfully",
                Id = workout.Id,
                Type = workout.Type,
                DurationMinutes = workout.DurationMinutes,
                Status = workout.Status,
                CaloriesBurned = workout.CaloriesBurned,
                Date = workout.Date,
                Notes = workout.Notes
            };
        }

        // Deletes a workout for the specified user.
        // If the workout is not found or does not belong to the user, no action is taken.
        public async Task DeleteWorkoutAsync(Guid userId, Guid workoutId)
        {
            var workout = await _dal.GetWorkoutByIdAsync(workoutId);
            if (workout == null || workout.UserId != userId) return;

            await _dal.DeleteWorkoutAsync(workoutId);
        }
    }
}
