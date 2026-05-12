using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.Services;


// AuthService is responsible for handling user authentication, including login, registration, and logout operations.
// It interacts with the ApiClient to communicate with the backend API and uses SecureStorageHelper to manage authentication tokens securely on the device.
public class AuthService : IAuthService
{
    private readonly ApiClient _apiClient;

    public bool IsLoggedIn { get; private set; }


    // Constructor that initializes the AuthService with an instance of ApiClient.
    public AuthService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Initializes the authentication state by checking if the user is already logged in based on the presence of a valid token in secure storage.
    public async Task InitializeAsync()
    {
        IsLoggedIn = await SecureStorageHelper.IsLoggedInAsync();
    }
    // Handles the login process by sending the user's email and password to the backend API.
    // If the login is successful and a token is received, it saves the token securely and updates the authentication state.
    public async Task<bool> LoginAsync(string email, string password)
    {
        try // Attempt to log in by sending the credentials to the API and handling the response.
        {
            var request = new LoginRequestDto
            {
                Email = email,
                Password = password
            };

            // Send the login request to the API and await the response.
            var response = await _apiClient.Client.LoginAsync(request);

            // If a token is received in the response, save it securely and update the authentication state to indicate that the user is logged in.
            if (response?.Token != null)
            {
                await SecureStorageHelper.SaveTokenAsync(response.Token); // Save the authentication token securely on the device.
                IsLoggedIn = true;
                return true;
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    // Handles the registration process by sending the user's details (first name, last name, email, password, and date of birth) to the backend API.
    // If the registration is successful and a token is received, it saves the token securely and updates the authentication state.
    public async Task<bool> RegisterAsync(string firstName, string lastName,string email, string password,DateTime dateOfBirth)
    {
        // Attempt to register by sending the user's details to the API and handling the response.
        try
        {
            var request = new RegisterRequestDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                DateOfBirth = dateOfBirth
            };

            var response = await _apiClient.Client.RegisterAsync(request);

            if (response?.Token != null)
            {
                await SecureStorageHelper.SaveTokenAsync(response.Token);
                IsLoggedIn = true;
                return true;
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }


    // Handles the logout process by removing the authentication token from secure storage and updating the authentication state to indicate that the user is logged out.
    public async Task LogoutAsync()
    {
        SecureStorageHelper.RemoveToken();
        IsLoggedIn = false;
        await Task.CompletedTask; // Return a completed task since this method is asynchronous but does not perform any asynchronous operations.
    }



}