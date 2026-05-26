using FluentAssertions;
using Moq;
using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.Entities;
using Traqtiv.API.Services;
using Xunit;
using Traqtiv.API.Services.Interfaces;
using Traqtiv.API.Models.DTOs.Responses;

namespace Traqtiv.Mobile.Tests.Services;

public class DailyActivityServiceTests
{
    private readonly Mock<ISmartFitnessDal> _mockDal;
    private readonly Mock<IRecommendationService> _mockRecommendationService;
    private readonly DailyActivityService _service;

    public DailyActivityServiceTests()
    {
        _mockDal = new Mock<ISmartFitnessDal>();
        _mockRecommendationService = new Mock<IRecommendationService>();
        _service = new DailyActivityService(_mockDal.Object, _mockRecommendationService.Object);
    }

    [Fact]
    public async Task GetActivitiesAsync_ReturnsEmptyList_WhenNoActivities()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockDal.Setup(x => x.GetActivitiesByRangeAsync(userId,
            It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<DailyActivity>());

        // Act
        var result = await _service.GetActivitiesAsync(userId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact(Skip = "Pending HealthKit implementation")]
    public async Task AddDailyActivityAsync_NotifiesRecommendationService()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var activity = new DailyActivityDto
        {
            UserId = userId,
            Steps = 5000,
            CaloriesBurned = 200,
            ActiveMinutes = 30,
            DistanceKm = 3.5
        };

        // Act
        await _service.AddDailyActivityAsync(userId, activity);

        // Assert
        _mockRecommendationService.Verify(
            x => x.OnDailyActivityUpdatedAsync(userId),
            Times.Once);
    }

    [Fact]
    public async Task GetActivitySummaryAsync_ReturnsSummary_WhenActivitiesExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var activities = new List<DailyActivity>
        {
            new() { Steps = 5000, CaloriesBurned = 200,
                    ActiveMinutes = 30, DistanceKm = 3.5 },
            new() { Steps = 8000, CaloriesBurned = 350,
                    ActiveMinutes = 45, DistanceKm = 5.0 }
        };

        _mockDal.Setup(x => x.GetActivitiesByRangeAsync(userId,
            It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(activities);

        // Act
        var result = await _service.GetActivitySummaryAsync(
            userId, DateTime.Today.AddDays(-7), DateTime.Today);

        // Assert
        result.TotalSteps.Should().Be(13000);
        result.TotalCaloriesBurned.Should().Be(550);
        result.TotalActiveMinutes.Should().Be(75);
    }
}