using FluentAssertions;
using Moq;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;
using Traqtiv.Mobile.ViewModels;
using Xunit;

namespace Traqtiv.Mobile.Tests.ViewModels;

public class HomeViewModelTests
{
    private readonly Mock<IWorkoutService> _mockWorkoutService;
    private readonly Mock<IDailyActivityService> _mockActivityService;
    private readonly Mock<IRecommendationService> _mockRecommendationService;
    private readonly Mock<INavigationService> _mockNavigationService;
    private readonly Mock<IWeatherService> _mockWeatherService;
    private readonly HomeViewModel _viewModel;

    public HomeViewModelTests()
    {
        _mockWorkoutService = new Mock<IWorkoutService>();
        _mockActivityService = new Mock<IDailyActivityService>();
        _mockRecommendationService = new Mock<IRecommendationService>();
        _mockNavigationService = new Mock<INavigationService>();
        _mockWeatherService = new Mock<IWeatherService>();

        _viewModel = new HomeViewModel(
            _mockWorkoutService.Object,
            _mockActivityService.Object,
            _mockRecommendationService.Object,
            _mockNavigationService.Object,
            _mockWeatherService.Object);
    }

    [Fact]
    public void ActiveTab_IsSetToHome_OnInitialization()
    {
        _viewModel.ActiveTab.Should().Be("home");
    }

    [Fact]
    public async Task LoadDataCommand_SetsRecentWorkouts_WhenWorkoutsExist()
    {
        // Arrange
        var workouts = new List<WorkoutDto>
        {
            new() { Id = Guid.NewGuid(), Type = WorkoutType.Strength, Date = DateTimeOffset.Now },
            new() { Id = Guid.NewGuid(), Type = WorkoutType.Cardio, Date = DateTimeOffset.Now.AddDays(-1) },
            new() { Id = Guid.NewGuid(), Type = WorkoutType.Flexibility, Date = DateTimeOffset.Now.AddDays(-2) },
            new() { Id = Guid.NewGuid(), Type = WorkoutType.Strength, Date = DateTimeOffset.Now.AddDays(-3) }
        };

        _mockWorkoutService.Setup(x => x.GetWorkoutsAsync()).ReturnsAsync(workouts);
        _mockActivityService.Setup(x => x.GetActivitiesAsync()).ReturnsAsync(new List<DailyActivityDto>());
        _mockRecommendationService.Setup(x => x.GetAlertsAsync()).ReturnsAsync(new List<AlertDto>());


        // Act
        await _viewModel.LoadDataCommand.ExecuteAsync(null);

        // Assert
        _viewModel.RecentWorkouts.Should().HaveCount(3); // Max 3
        _viewModel.IsBusy.Should().BeFalse();
    }

    [Fact]
    public async Task LoadDataCommand_SetsTodayActivity_WhenActivityExists()
    {
        // Arrange
        var todayActivity = new DailyActivityDto
        {
            Steps = 5000,
            CaloriesBurned = 200,
            Date = DateTimeOffset.Now
        };

        _mockWorkoutService.Setup(x => x.GetWorkoutsAsync()).ReturnsAsync(new List<WorkoutDto>());
        _mockActivityService.Setup(x => x.GetActivitiesAsync()).ReturnsAsync(new List<DailyActivityDto> { todayActivity });
        _mockRecommendationService.Setup(x => x.GetAlertsAsync()).ReturnsAsync(new List<AlertDto>());

        // Act
        await _viewModel.LoadDataCommand.ExecuteAsync(null);

        // Assert
        _viewModel.TodayActivity.Should().NotBeNull();
        _viewModel.TodayActivity!.Steps.Should().Be(5000);
    }



}