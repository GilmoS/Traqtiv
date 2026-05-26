using FluentAssertions;
using Moq;
using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.Entities;
using Traqtiv.API.Models.Enums;
using Traqtiv.API.Services;
using Traqtiv.API.Services.Interfaces;

using Xunit;

namespace Traqtiv.Mobile.Tests.Services;

public class WorkoutServiceTests
{
    private readonly Mock<ISmartFitnessDal> _mockDal;
    private readonly Mock<IRecommendationService> _mockRecommendationService;
    private readonly WorkoutService _service;

    public WorkoutServiceTests()
    {
        _mockDal = new Mock<ISmartFitnessDal>();
        _mockRecommendationService = new Mock<IRecommendationService>();
        _service = new WorkoutService(_mockDal.Object, _mockRecommendationService.Object);
    }

    [Fact]
    public async Task GetWorkoutsAsync_ReturnsEmptyList_WhenNoWorkouts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockDal.Setup(x => x.GetWorkoutsByUserIdAsync(userId))
            .ReturnsAsync(new List<Workout>());

        // Act
        var result = await _service.GetWorkoutsAsync(userId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetWorkoutsAsync_ReturnsMappedDtos_WhenWorkoutsExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var workouts = new List<Workout>
        {
            new() { Id = Guid.NewGuid(), UserId = userId,
                    Type = WorkoutType.Strength,
                    DurationMinutes = 60,
                    Status = WorkoutStatus.Completed,
                    CaloriesBurned = 300 }
        };
        _mockDal.Setup(x => x.GetWorkoutsByUserIdAsync(userId))
            .ReturnsAsync(workouts);

        // Act
        var result = await _service.GetWorkoutsAsync(userId);

        // Assert
        result.Should().HaveCount(1);
        result[0].DurationMinutes.Should().Be(60);
        result[0].CaloriesBurned.Should().Be(300);
    }

    [Fact]
    public async Task AddWorkoutAsync_NotifiesRecommendationService()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new AddWorkoutDto
        {
            Type = WorkoutType.Cardio,
            DurationMinutes = 45,
            Status = WorkoutStatus.Completed
        };

        // Act
        await _service.AddWorkoutAsync(userId, request);

        // Assert
        _mockRecommendationService.Verify(x => x.OnWorkoutAddedAsync(userId),Times.Once);
    }


    [Fact]
    public async Task DeleteWorkoutAsync_CallsDal()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var workoutId = Guid.NewGuid();
        _mockDal.Setup(x => x.GetWorkoutByIdAsync(workoutId)).ReturnsAsync(new Workout { Id = workoutId, UserId = userId });
        // Act
        await _service.DeleteWorkoutAsync(userId, workoutId);

        // Assert
        _mockDal.Verify(x => x.DeleteWorkoutAsync(workoutId), Times.Once);
    }
}
