using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;
using Traqtiv.API.Models.Enums;
using Traqtiv.Mobile.Tests.Integration.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Traqtiv.Mobile.Tests.Integration;

public class WorkoutIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public WorkoutIntegrationTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    // SetAuthHeader is a helper method to set the Authorization header with a Bearer token for authenticated requests
    //every test that requires authentication can call this method with the appropriate token to ensure the request is properly authenticated
    private void SetAuthHeader(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // JsonSerializerOptions with a JsonStringEnumConverter is defined to ensure that enum values are serialized and deserialized as their string representations in JSON,
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    // ───── GET /api/workout ─────

    // Test to verify that an authenticated user can successfully retrieve their workouts and receives a 200 OK response
    [Fact]
    public async Task GetWorkouts_Authenticated_Returns200()
    {
        var token = await AuthHelper.RegisterAndGetTokenAsync(_client, "getworkouts@example.com");
        SetAuthHeader(token);

        var response = await _client.GetAsync("/api/workout");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // test to verify that an unauthenticated request to retrieve workouts returns a 401 Unauthorized response,
    // ensuring that the endpoint is protected and requires authentication
    [Fact]
    public async Task GetWorkouts_Unauthenticated_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;

        var response = await _client.GetAsync("/api/workout");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // ───── POST /api/workout ─────

    //verify that a valid workout can be added successfully,
    //returning a 200 OK response along with the ID of the newly created workout,
    //confirming that the endpoint correctly processes valid input and creates a new workout entry in the system
    [Fact]
    public async Task AddWorkout_ValidData_Returns200WithId()
    {
        var token = await AuthHelper.RegisterAndGetTokenAsync(_client, "addworkout@example.com");
        SetAuthHeader(token);

        var request = new AddWorkoutDto
        {
            Type = WorkoutType.Cardio,
            DurationMinutes = 30,
            CaloriesBurned = 300,
            Notes = "Morning run",
            Date = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/workout", request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var raw = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<WorkoutDto>(raw, _jsonOptions);
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
    }


    [Fact]
    public async Task AddWorkout_Unauthenticated_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;

        var request = new AddWorkoutDto
        {
            Type = WorkoutType.Cardio,
            DurationMinutes = 30,
            CaloriesBurned = 300,
            Date = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/workout", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // ───── GET /api/workout/{id} ─────

    // בודק קבלת אימון לפי ID קיים
    [Fact]
    public async Task GetWorkoutById_ExistingId_Returns200()
    {
        var token = await AuthHelper.RegisterAndGetTokenAsync(_client, "getbyid@example.com");
        SetAuthHeader(token);

        // יוצר אימון תחילה
        var addRequest = new AddWorkoutDto
        {
            Type = WorkoutType.Cardio,
            DurationMinutes = 45,
            CaloriesBurned = 400,
            Date = DateTime.UtcNow
        };
        var addResponse = await _client.PostAsJsonAsync("/api/workout", addRequest);
        var created = await addResponse.Content.ReadFromJsonAsync<WorkoutDto>(_jsonOptions);

        // מביא לפי ID
        var response = await _client.GetAsync($"/api/workout/{created!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // בודק קבלת אימון עם ID שלא קיים — מצפים ל-404
    [Fact]
    public async Task GetWorkoutById_NonExistentId_Returns404()
    {
        var token = await AuthHelper.RegisterAndGetTokenAsync(_client, "getbyid2@example.com");
        SetAuthHeader(token);

        var response = await _client.GetAsync($"/api/workout/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ───── DELETE /api/workout/{id} ─────

    // בודק מחיקת אימון קיים
    [Fact]
    public async Task DeleteWorkout_ExistingId_Returns200()
    {
        var token = await AuthHelper.RegisterAndGetTokenAsync(_client, "delete@example.com");
        SetAuthHeader(token);

        var addRequest = new AddWorkoutDto
        {
            Type = WorkoutType.Cardio,
            DurationMinutes = 60,
            CaloriesBurned = 500,
            Date = DateTime.UtcNow
        };
        var addResponse = await _client.PostAsJsonAsync("/api/workout", addRequest);
        var created = await addResponse.Content.ReadFromJsonAsync<WorkoutDto>(_jsonOptions);

        var response = await _client.DeleteAsync($"/api/workout/{created!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // בודק שמשתמש אחד לא יכול למחוק אימון של משתמש אחר
    [Fact]
    public async Task DeleteWorkout_OtherUsersWorkout_Returns403Or404()
    {
        // first user creates a workout
        var token1 = await AuthHelper.RegisterAndGetTokenAsync(_client, "user1@example.com");
        SetAuthHeader(token1);

        var addRequest = new AddWorkoutDto
        {
            Type = WorkoutType.Strength,
            DurationMinutes = 30,
            CaloriesBurned = 150,
            Date = DateTime.UtcNow
        };
        var addResponse = await _client.PostAsJsonAsync("/api/workout", addRequest);
        var created = await addResponse.Content.ReadFromJsonAsync<WorkoutDto>(_jsonOptions);

        //Sevond user tries to delete the first user's workout
        var token2 = await AuthHelper.RegisterAndGetTokenAsync(_client, "user2@example.com");
        SetAuthHeader(token2);

        var response = await _client.DeleteAsync($"/api/workout/{created!.Id}");

        Assert.True(response.StatusCode == HttpStatusCode.Forbidden ||response.StatusCode == HttpStatusCode.NotFound);
    }
}
