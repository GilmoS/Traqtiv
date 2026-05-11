namespace Traqtiv.Mobile.Services.Interfaces;

// Interface for workout-related services, providing methods to manage workouts, including retrieval, addition, updating, and deletion.

public interface IWorkoutService : IService
{
    // Asynchronously retrieves a list of all workouts, returning a list of WorkoutDto objects.
    Task<List<WorkoutDto>> GetWorkoutsAsync();
    
    // Asynchronously retrieves a workout by its unique identifier, returning a WorkoutDto if found, or null if not found.
    Task<WorkoutDto?> GetWorkoutByIdAsync(Guid id);
    
    // Asynchronously adds a new workout with the provided details, returning true if the addition was successful.
    Task<bool> AddWorkoutAsync(AddWorkoutDto request);

    // Asynchronously updates an existing workout identified by its unique identifier with the provided details, returning true if the update was successful.
    Task<bool> UpdateWorkoutAsync(Guid id, UpdateWorkoutDto request);

    // Asynchronously deletes a workout by its unique identifier, returning true if the deletion was successful.
    Task<bool> DeleteWorkoutAsync(Guid id);
}