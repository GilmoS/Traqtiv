using Moq;
using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Models.Entities;

using FluentAssertions;
using Moq;
using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Models.Entities;
using Traqtiv.API.Services;
using Xunit;

namespace Traqtiv.Mobile.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<ISmartFitnessDal> _mockDal;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockDal = new Mock<ISmartFitnessDal>();
        _service = new UserService(_mockDal.Object);
    }

    [Fact]
    public async Task GetProfileAsync_ReturnsProfile_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "Nir",
            LastName = "Peper",
            Email = "test@test.com",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        _mockDal.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _service.GetProfileAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("Nir");
        result.LastName.Should().Be("Peper");
        result.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task GetProfileAsync_ReturnsNull_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockDal.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _service.GetProfileAsync(userId);

        // Assert
        result!.Success.Should().BeFalse();
        result.Message.Should().Be("User not found");
    }

    [Fact(Skip = "Pending implementation")]

    public async Task AddBodyMetricsAsync_CallsDal()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var metrics = new BodyMetrics
        {
            UserId = userId,
            Weight = 80.0,
            RestingHeartRate = 60,
            BMI = 24.2
        };

        // Act
        await _service.AddBodyMetricsAsync(userId, metrics);

        // Assert
        _mockDal.Verify(x => x.AddMetricsAsync(It.Is<BodyMetrics>(m => m.UserId == userId)),Times.Once);
    }
}