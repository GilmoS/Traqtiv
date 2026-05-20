using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.Services;

// This service is responsible for handling user-related operations, such as retrieving and updating user profiles.
public class UserService : IUserService
{
    private readonly ApiClient _apiClient;

    // Constructor that initializes the UserService with an instance of ApiClient.
    public UserService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Initializes the service asynchronously.
    // In this case, it simply completes immediately, but it can be extended to perform any necessary setup.
    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    // Retrieves the user's profile asynchronously.
    // It attaches the authentication token to the API client and then calls the ProfileGETAsync method to fetch the profile data.
    // If an exception occurs, it returns null.
    public async Task<UserProfileDto?> GetProfileAsync()
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            return await _apiClient.Client.ProfileGETAsync();// This method is expected to return a UserProfileDto object containing the user's profile information.
        }
        catch (Exception)
        {
            return null;
        }
    }

    // Updates the user's profile asynchronously.
    // It takes an UpdateProfileDto object as a parameter, which contains the updated profile information.
    public async Task<bool> UpdateProfileAsync(UpdateProfileDto request)
    {
        try
        {
            await _apiClient.AttachTokenAsync();
            await _apiClient.Client.ProfilePUTAsync(request);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}