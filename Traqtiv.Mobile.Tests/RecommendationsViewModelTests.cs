using FluentAssertions;
using Moq;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;
using Traqtiv.Mobile.ViewModels;
using Xunit;

namespace Traqtiv.Mobile.Tests.ViewModels;

public class RecommendationsViewModelTests
{
    private readonly Mock<IRecommendationService> _mockRecommendationService;
    private readonly RecommendationsViewModel _viewModel;

    public RecommendationsViewModelTests()
    {
        _mockRecommendationService = new Mock<IRecommendationService>();
        _viewModel = new RecommendationsViewModel(_mockRecommendationService.Object);
    }

    [Fact]
    public async Task LoadDataCommand_SetsRecommendations_WhenDataExists()
    {
        // Arrange
        var recommendations = new List<RecommendationDto>
        {
            new() { Id = Guid.NewGuid(), Message = "Take a walk today!" },
            new() { Id = Guid.NewGuid(), Message = "Rest day recommended" }
        };
        _mockRecommendationService.Setup(x => x.GetRecommendationsAsync()).ReturnsAsync(recommendations);
        _mockRecommendationService.Setup(x => x.GetAlertsAsync()).ReturnsAsync( new List<AlertDto>());

        // Act
        await _viewModel.LoadDataCommand.ExecuteAsync(null);

        // Assert
        _viewModel.Recommendations.Should().HaveCount(2);
        _viewModel.IsBusy.Should().BeFalse();
    }

    [Fact]
    public async Task LoadDataCommand_SetsAlerts_WhenAlertsExist()
    {
        // Arrange
        _mockRecommendationService.Setup(x => x.GetRecommendationsAsync()).ReturnsAsync(new List<RecommendationDto>());
        _mockRecommendationService.Setup(x => x.GetAlertsAsync()).ReturnsAsync(new List<AlertDto>
            {
                new() { Id = Guid.NewGuid(), Message = "Overload alert!", IsRead = false }
            });

        // Act
        await _viewModel.LoadDataCommand.ExecuteAsync(null);

        // Assert
        _viewModel.Alerts.Should().HaveCount(1);
    }

    [Fact(Skip = "Requires MAUI runtime - tested in UI tests")]

    public async Task MarkAlertAsReadCommand_ReloadsData_AfterSuccess()
    {
        // Arrange
        var alertId = Guid.NewGuid();
        var alert = new AlertDto { Id = alertId, IsRead = false };

        _mockRecommendationService.Setup(x => x.MarkAlertAsReadAsync(alertId)).ReturnsAsync(true);
        _mockRecommendationService.Setup(x => x.GetRecommendationsAsync()).ReturnsAsync(new List<RecommendationDto>());
        _mockRecommendationService.Setup(x => x.GetAlertsAsync()).ReturnsAsync(new List<AlertDto>());

        // Act
        await _viewModel.MarkAlertAsReadCommand.ExecuteAsync(alert);

        // Assert
        _mockRecommendationService.Verify(x => x.GetAlertsAsync(), Times.AtLeastOnce);
    }
}
