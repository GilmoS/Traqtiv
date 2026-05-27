using FluentAssertions;
using Moq;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services;
using Traqtiv.Mobile.Services.Interfaces;
using Traqtiv.Mobile.ViewModels;
using Xunit;

namespace Traqtiv.Mobile.Tests.ViewModels;

public class BodyMetricsViewModelTests
{
    private readonly Mock<IBodyMetricsService> _mockMetricsService;
    private readonly Mock<HealthService> _mockHealthService;
    private readonly BodyMetricsViewModel _viewModel;

    public BodyMetricsViewModelTests()
    {
        _mockMetricsService = new Mock<IBodyMetricsService>();
        _mockHealthService = new Mock<HealthService>();

        _viewModel = new BodyMetricsViewModel(_mockMetricsService.Object,_mockHealthService.Object);
    }

    [Fact]
    public async Task LoadDataCommand_SetsLatestMetrics_WhenDataExists()
    {
        // Arrange
        var metrics = new List<BodyMetricsDto>
        {
            new() { Weight = 80.0, RestingHeartRate = 60, Bmi = 24.2,MeasuredAt = DateTimeOffset.Now },
            new() { Weight = 82.0, RestingHeartRate = 62, Bmi = 25.0,MeasuredAt = DateTimeOffset.Now.AddDays(-7) }
        };

        _mockMetricsService.Setup(x => x.GetMetricsAsync()).ReturnsAsync(metrics);

        // Act
        await _viewModel.LoadDataCommand.ExecuteAsync(null);

        // Assert
        _viewModel.Weight.Should().Be(80.0);
        _viewModel.RestingHeartRate.Should().Be(60);
        _viewModel.Bmi.Should().Be(24.2);
        _viewModel.MetricsList.Should().HaveCount(2);
    }

    [Fact]
    public async Task LoadDataCommand_SetsZeroValues_WhenNoMetrics()
    {
        // Arrange
        _mockMetricsService.Setup(x => x.GetMetricsAsync()).ReturnsAsync(new List<BodyMetricsDto>());

        // Act
        await _viewModel.LoadDataCommand.ExecuteAsync(null);

        // Assert
        _viewModel.Weight.Should().Be(0);
        _viewModel.MetricsList.Should().BeEmpty();
    }
}