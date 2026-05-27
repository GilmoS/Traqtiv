using System.Net;
using System.Net.Http.Json;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;

namespace Traqtiv.Mobile.Tests.Integration;

public class AuthIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    // ───── Register ─────

    [Fact]
    public async Task Register_ValidData_Returns200WithToken()
    {
        var request = new RegisterRequestDto
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Password = "Password123!",
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
        Assert.NotEqual(Guid.Empty, result.UserId);
    }

    [Fact]
    public async Task Register_DuplicateEmail_Returns400()
    {
        var request = new RegisterRequestDto
        {
            FirstName = "User",
            LastName = "One",
            Email = "duplicate@example.com",
            Password = "Password123!",
        };

        await _client.PostAsJsonAsync("/api/auth/register", request);
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_MissingFields_Returns400()
    {
        var request = new { Name = "Test" };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ───── Login ─────

    [Fact]
    public async Task Login_ValidCredentials_Returns200WithToken()
    {
        var registerRequest = new RegisterRequestDto
        {
            FirstName = "Login",
            LastName = "Test User",             
            Email = "login@example.com",
            Password = "Password123!",
        };
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginRequestDto
        {
            Email = "login@example.com",
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task Login_WrongPassword_Returns401()
    {
        var registerRequest = new RegisterRequestDto
        {
            FirstName = "Wrong",
            LastName = "Pass User", 
            Email = "wrongpass@example.com",
            Password = "Password123!",
        };
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginRequestDto
        {
            Email = "wrongpass@example.com",
            Password = "WrongPassword!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_NonExistentUser_Returns401()
    {
        var loginRequest = new LoginRequestDto
        {
            Email = "nobody@example.com",
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}