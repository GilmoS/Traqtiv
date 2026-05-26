using FluentAssertions;
using Moq;
using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Models.Entities;
using Traqtiv.API.Models.Enums;
using Traqtiv.API.Services;
using Xunit;

namespace Traqtiv.Mobile.Tests.Services;

public class RecommendationServiceTests
{
    private readonly Mock<ISmartFitnessDal> _mockDal;
    private readonly RecommendationService _service;

    public RecommendationServiceTests()
    {
        _mockDal = new Mock<ISmartFitnessDal>();
        _service = new RecommendationService(_mockDal.Object);
    }

    [Fact]
    public async Task GetRecommendationsAsync_ReturnsEmptyList_WhenNoRecommendations()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockDal.Setup(x => x.GetRecommendationsByUserIdAsync(userId))
            .ReturnsAsync(new List<Recommendation>());

        // Act
        var result = await _service.GetRecommendationsAsync(userId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRecommendationsAsync_ReturnsMappedDtos_WhenRecommendationsExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var recommendations = new List<Recommendation>
        {
            new() { Id = Guid.NewGuid(), UserId = userId,
                    Type = RecommendationType.Inactivity,
                    Message = "Test message", IsRead = false }
        };
        _mockDal.Setup(x => x.GetRecommendationsByUserIdAsync(userId))
            .ReturnsAsync(recommendations);

        // Act
        var result = await _service.GetRecommendationsAsync(userId);

        // Assert
        result.Should().HaveCount(1);
        result[0].Message.Should().Be("Test message");
        result[0].Success.Should().BeTrue();
    }

    [Fact]
    public async Task GetAlertsAsync_ReturnsEmptyList_WhenNoAlerts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockDal.Setup(x => x.GetAlertsByUserIdAsync(userId))
            .ReturnsAsync(new List<Alert>());

        // Act
        var result = await _service.GetAlertsAsync(userId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task OnWorkoutAddedAsync_CreatesOverloadAlert_WhenMoreThan5Workouts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var workouts = Enumerable.Range(0, 6).Select(_ => new Workout
        {
            UserId = userId,
            Date = DateTime.Today,
            Type = WorkoutType.Strength,
            Status = WorkoutStatus.Completed
        }).ToList();

        _mockDal.Setup(x => x.GetWorkoutsByUserIdAsync(userId))
            .ReturnsAsync(workouts);

        // Act
        await _service.OnWorkoutAddedAsync(userId);

        // Assert
        _mockDal.Verify(x => x.AddAlertAsync(
            It.Is<Alert>(a => a.Type == AlertType.Overload)),
            Times.Once);
    }
}
