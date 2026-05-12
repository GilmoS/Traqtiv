using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.Services;

// WorkoutService is responsible for managing workout-related operations, including retrieving, adding, updating, and deleting workouts.
// It interacts with the ApiClient to communicate with the backend API and ensures that the authentication token is included in the request headers for authorized access.
public class WorkoutService : IWorkoutService
{
    private readonly ApiClient _apiClient;

    // Constructor that initializes the WorkoutService with an instance of ApiClient.
    public WorkoutService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Initializes the workout service.
    // In this implementation, there are no specific initialization steps required, but the method is defined to comply with the IWorkoutService interface.
    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    // Retrieves a list of all workouts from the backend API.
    public async Task<List<WorkoutDto>> GetWorkoutsAsync()
    {
        // Attempt to retrieve the list of workouts by attaching the authentication token and making the API call.
        try
        {
            await _apiClient.AttachTokenAsync();
            var result = await _apiClient.Client.WorkoutsAllAsync();
            return result?.ToList() ?? new List<WorkoutDto>(); // Return the list of workouts if the result is not null
        }
        catch (Exception)
        {
            return new List<WorkoutDto>(); // Return an empty list if an error occurs during the API call.
        }
    }

    // Retrieves a specific workout by its unique identifier (ID) from the backend API.
    public async Task<WorkoutDto?> GetWorkoutByIdAsync(Guid id)
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            return await _apiClient.Client.WorkoutsGETAsync(id);
        }
        catch (Exception)
        {
            return null;
        }
    }

    // Adds a new workout to the backend API using the provided AddWorkoutDto request object.
    public async Task<bool> AddWorkoutAsync(AddWorkoutDto request)
    {
        // Attempt to add a new workout by attaching the authentication token and making the API call with the provided request data.
        try
        {
            await _apiClient.AttachTokenAsync();
            await _apiClient.Client.WorkoutsPOSTAsync(request);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    // Updates an existing workout in the backend API using the provided UpdateWorkoutDto request object and the workout's unique identifier (ID).
    public async Task<bool> UpdateWorkoutAsync(Guid id, UpdateWorkoutDto request)
    {
        // Attempt to update an existing workout by attaching the authentication token and making the API call with the provided request data and workout ID.
        try
        {
            await _apiClient.AttachTokenAsync();
            await _apiClient.Client.WorkoutsPUTAsync(id, request);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    // Deletes a workout from the backend API using the workout's unique identifier (ID).
    public async Task<bool> DeleteWorkoutAsync(Guid id)
    {
        // Attempt to delete a workout by attaching the authentication token and making the API call with the provided workout ID.
        try
        {
            await _apiClient.AttachTokenAsync();
            await _apiClient.Client.WorkoutsDELETEAsync(id);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
