using Traqtiv.Mobile.Models;

namespace Traqtiv.Mobile.Services.Interfaces;

//This service is responsible for handling user-related operations, such as retrieving and updating user profiles.
public interface IUserService : IService
{
    // Retrieves the user's profile information asynchronously.
    Task<UserProfileDto?> GetProfileAsync();

    // Updates the user's profile information asynchronously.
    Task<bool> UpdateProfileAsync(UpdateProfileDto request);
}
