using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Traqtiv.Mobile.Tests.Integration.Helpers;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;


namespace Traqtiv.Mobile.Tests.Integration;

public class UserIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    public UserIntegrationTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private void SetAuthHeader(string token)
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    // ───── GET /api/user/profile ─────

    // Checks that an authenticated user can retrieve their profile successfully
    [Fact]
    public async Task GetProfile_Authenticated_Returns200WithProfile()
    {
        var token = await AuthHelper.RegisterAndGetTokenAsync(_client, "profile@example.com");
        SetAuthHeader(token);

        var response = await _client.GetAsync("/api/user/profile");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var raw = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<UserProfileDto>(raw, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal("profile@example.com", result.Email);
    }

    // Checks that an un-authenticated user receives a 401 Unauthorized response when trying to access the profile endpoint
    [Fact]
    public async Task GetProfile_Unauthenticated_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;

        var response = await _client.GetAsync("/api/user/profile");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // ───── PUT /api/user/profile ─────

    // Checks that an authenticated user can update their profile successfully and that the changes are reflected in the response
    [Fact]
    public async Task UpdateProfile_ValidData_Returns200WithUpdatedProfile()
    {
        var token = await AuthHelper.RegisterAndGetTokenAsync(_client, "updateprofile@example.com");
        SetAuthHeader(token);

        var request = new UpdateProfileDto
        {
            FirstName = "Updated",
            LastName = "Name",
            DateOfBirth = new DateTime(1990, 5, 15)
        };

        var response = await _client.PutAsJsonAsync("/api/user/profile", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var raw = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<UserProfileDto>(raw, _jsonOptions);
        Assert.NotNull(result);
        Assert.Equal("Updated", result.FirstName);
        Assert.Equal("Name", result.LastName);
    }

    // Checks that an un-authenticated user receives a 401 Unauthorized response when trying to update the profile
    [Fact]
    public async Task UpdateProfile_Unauthenticated_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;

        var request = new UpdateProfileDto
        {
            FirstName = "Updated",
            LastName = "Name",
            DateOfBirth = new DateTime(1990, 5, 15)
        };

        var response = await _client.PutAsJsonAsync("/api/user/profile", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // ───── GET /api/user/metrics ─────

    //Checks that an authenticated user can retrieve their body metrics successfully and receives a 200 OK response
    [Fact]
    public async Task GetBodyMetrics_Authenticated_Returns200()
    {
        var token = await AuthHelper.RegisterAndGetTokenAsync(_client, "metrics@example.com");
        SetAuthHeader(token);

        var response = await _client.GetAsync("/api/user/metrics");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // Checks that an un-authenticated user receives a 401 Unauthorized response when trying to access the body metrics endpoint
    [Fact]
    public async Task GetBodyMetrics_Unauthenticated_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;

        var response = await _client.GetAsync("/api/user/metrics");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // ───── POST /api/user/metrics ─────

    // Checks that an authenticated user can add new body metrics successfully and receives a 200 OK response
    [Fact]
    public async Task AddBodyMetrics_ValidData_Returns200()
    {
        var token = await AuthHelper.RegisterAndGetTokenAsync(_client, "addmetrics@example.com");
        SetAuthHeader(token);

        var request = new AddMetricsDto
        {
            Weight = 75.5,
            RestingHeartRate = 65,
            BMI = 23.4
        };

        var response = await _client.PostAsJsonAsync("/api/user/metrics", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // Checks that after adding body metrics, they are indeed saved and can be retrieved
    [Fact]
    public async Task AddBodyMetrics_ThenGet_ReturnsAddedMetrics()
    {
        var token = await AuthHelper.RegisterAndGetTokenAsync(_client, "metricsflow@example.com");
        SetAuthHeader(token);

        var request = new AddMetricsDto
        {
            Weight = 80.0,
            RestingHeartRate = 70,
            BMI = 24.5
        };

        await _client.PostAsJsonAsync("/api/user/metrics", request);

        var response = await _client.GetAsync("/api/user/metrics");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var raw = await response.Content.ReadAsStringAsync();
        var results = JsonSerializer.Deserialize<List<BodyMetricsDto>>(raw, _jsonOptions);
        Assert.NotNull(results);
        Assert.NotEmpty(results);
        Assert.Equal(80.0, results.First().Weight);
    }

    // Checks that an un-authenticated user receives a 401 Unauthorized response when trying to add body metrics
    [Fact]
    public async Task AddBodyMetrics_Unauthenticated_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;

        var request = new AddMetricsDto
        {
            Weight = 75.5,
            RestingHeartRate = 65,
            BMI = 23.4
        };

        var response = await _client.PostAsJsonAsync("/api/user/metrics", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
