using System.Net.Http.Json;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;

namespace Traqtiv.Mobile.Tests.Integration.Helpers;

public static class AuthHelper
{
    // Registers a new user and retrieves the authentication token for that user
    // This method is for the integration tests that require an authenticated user
    public static async Task<string> RegisterAndGetTokenAsync(
        HttpClient client,
        string email = "workout@example.com",
        string password = "Password123!")
    {
        var request = new RegisterRequestDto
        {
            FirstName = "Test",
            LastName = "User",
            Email = email,
            Password = password,
            DateOfBirth = new DateTime(1995, 1, 1)
        };

        var response = await client.PostAsJsonAsync("/api/auth/register", request);
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        return result!.Token;
    }
}