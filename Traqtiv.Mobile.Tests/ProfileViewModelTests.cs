using FluentAssertions;
using Moq;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;
using Traqtiv.Mobile.ViewModels;
using Xunit;

namespace Traqtiv.Mobile.Tests.ViewModels;

public class ProfileViewModelTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<INavigationService> _mockNavigationService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IWorkoutService> _mockWorkoutService;
    private readonly Mock<IDailyActivityService> _mockActivityService;
    private readonly ProfileViewModel _viewModel;

    public ProfileViewModelTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockNavigationService = new Mock<INavigationService>();
        _mockUserService = new Mock<IUserService>();
        _mockWorkoutService = new Mock<IWorkoutService>();
        _mockActivityService = new Mock<IDailyActivityService>();

        _viewModel = new ProfileViewModel(
            _mockAuthService.Object,
            _mockNavigationService.Object,
            _mockUserService.Object,
            _mockWorkoutService.Object,
            _mockActivityService.Object);
    }

    [Fact]
    public void ActiveTab_IsSetToProfile_OnInitialization()
    {
        _viewModel.Title.Should().Be("Profile");
    }

    [Fact]
    public async Task LoadDataCommand_SetsProfileData_WhenUserExists()
    {
        // Arrange
        var profile = new UserProfileDto
        {
            FirstName = "Nir",
            LastName = "Peper",
            Email = "test@test.com",
            DateOfBirth = DateTimeOffset.Now.AddYears(-25)
        };

        _mockUserService.Setup(x => x.GetProfileAsync()).ReturnsAsync(profile);
        _mockWorkoutService.Setup(x => x.GetWorkoutsAsync()).ReturnsAsync(new List<WorkoutDto>());
        _mockActivityService.Setup(x => x.GetActivitySummaryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new ActivitySummaryDto());

        // Act
        await _viewModel.LoadDataCommand.ExecuteAsync(null);

        // Assert
        _viewModel.FirstName.Should().Be("Nir");
        _viewModel.LastName.Should().Be("Peper");
        _viewModel.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task LoadDataCommand_SetsTotalWorkouts_FromWorkoutsList()
    {
        // Arrange
        _mockUserService.Setup(x => x.GetProfileAsync()).ReturnsAsync(new UserProfileDto());
        _mockWorkoutService.Setup(x => x.GetWorkoutsAsync()).ReturnsAsync(new List<WorkoutDto>
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            });
        _mockActivityService.Setup(x => x.GetActivitySummaryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new ActivitySummaryDto());

        // Act
        await _viewModel.LoadDataCommand.ExecuteAsync(null);

        // Assert
        _viewModel.TotalWorkouts.Should().Be(3);
    }
}
