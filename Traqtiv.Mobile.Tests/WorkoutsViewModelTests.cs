using FluentAssertions;
using Moq;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;
using Traqtiv.Mobile.ViewModels;
using Xunit;

namespace Traqtiv.Mobile.Tests.ViewModels;

public class WorkoutsViewModelTests
{
    private readonly Mock<IWorkoutService> _mockWorkoutService;
    private readonly Mock<INavigationService> _mockNavigationService;
    private readonly WorkoutsViewModel _viewModel;

    public WorkoutsViewModelTests()
    {
        _mockWorkoutService = new Mock<IWorkoutService>();

        _mockNavigationService = new Mock<INavigationService>();

        _viewModel = new WorkoutsViewModel(_mockWorkoutService.Object,_mockNavigationService.Object);
    }

    [Fact]
    public async Task LoadWorkoutsCommand_SetsWorkouts_WhenServiceReturnsData()
    {
        // Arrange
        var workouts = new List<WorkoutDto>
        {
            new() { Id = Guid.NewGuid(), Type = WorkoutType.Strength,DurationMinutes = 60, CaloriesBurned = 300 },
            new() { Id = Guid.NewGuid(), Type = WorkoutType.Cardio,DurationMinutes = 45, CaloriesBurned = 200 }
        };
        _mockWorkoutService.Setup(x => x.GetWorkoutsAsync()).ReturnsAsync(workouts);

        // Act
        await _viewModel.LoadWorkoutsCommand.ExecuteAsync(null);

        // Assert
        _viewModel.Workouts.Should().HaveCount(2);
        _viewModel.IsBusy.Should().BeFalse();
    }

    [Fact]
    public async Task LoadWorkoutsCommand_SetsEmptyList_WhenNoWorkouts()
    {
        // Arrange
        _mockWorkoutService.Setup(x => x.GetWorkoutsAsync()).ReturnsAsync(new List<WorkoutDto>());

        // Act
        await _viewModel.LoadWorkoutsCommand.ExecuteAsync(null);

        // Assert
        _viewModel.Workouts.Should().BeEmpty();
        _viewModel.IsBusy.Should().BeFalse();
    }

    [Fact]
    public void ActiveTab_IsSetToWorkouts_OnInitialization()
    {
        // Assert
        _viewModel.ActiveTab.Should().Be("workouts");
    }
}