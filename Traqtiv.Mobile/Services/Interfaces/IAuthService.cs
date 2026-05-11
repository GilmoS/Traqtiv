namespace Traqtiv.Mobile.Services.Interfaces;

// Interface for authentication services, providing methods for user login, registration, and logout.
public interface IAuthService : IService
{
    bool IsLoggedIn { get; }// Indicates whether a user is currently logged in.

    // Asynchronously attempts to log in a user with the provided email and password, returning true if successful.
    Task<bool> LoginAsync(string email, string password); 

    // Asynchronously attempts to register a new user with the provided details, returning true if successful.
    Task<bool> RegisterAsync(string firstName, string lastName,string email, string password, DateTime dateOfBirth);

    // Asynchronously logs out the current user, clearing any authentication state.
    Task LogoutAsync();
}