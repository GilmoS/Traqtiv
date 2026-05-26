using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Models.Entities;
using Traqtiv.API.Services;
using Xunit;

namespace Traqtiv.Mobile.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<ISmartFitnessDal> _mockDal;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _mockDal = new Mock<ISmartFitnessDal>();

        var configValues = new Dictionary<string, string?>
{
    {"JwtSettings:SecretKey", "TestSecretKeyThatIsLongEnough12345678"},
    {"JwtSettings:Issuer", "TestIssuer"},
    {"JwtSettings:Audience", "TestAudience"},
    {"JwtSettings:ExpirationHours", "24"}  
};

        var configuration = new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();

        _service = new AuthService(_mockDal.Object, configuration);
    }

    [Fact]
    public async Task LoginAsync_ReturnsToken_WhenCredentialsValid()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        };
        _mockDal.Setup(x => x.GetUserByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        // Act
        var result = await _service.LoginAsync(
            new API.Models.DTOs.Requests.LoginRequestDto
            {
                Email = "test@test.com",
                Password = "password123"
            });

        // Assert
        result.Success.Should().BeTrue();
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task LoginAsync_ReturnsFalse_WhenUserNotFound()
    {
        // Arrange
        _mockDal.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.LoginAsync(
            new API.Models.DTOs.Requests.LoginRequestDto
            {
                Email = "wrong@test.com",
                Password = "password"
            });

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterAsync_ReturnsToken_WhenNewUser()
    {
        // Arrange
        _mockDal.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var request = new API.Models.DTOs.Requests.RegisterRequestDto
        {
            FirstName = "Nir",
            LastName = "Peper",
            Email = "new@test.com",
            Password = "password123",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        // Act
        var result = await _service.RegisterAsync(request);

        // Assert
        result.Success.Should().BeTrue();
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task RegisterAsync_ReturnsFalse_WhenEmailExists()
    {
        // Arrange
        _mockDal.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new User { Email = "existing@test.com" });

        var request = new API.Models.DTOs.Requests.RegisterRequestDto
        {
            Email = "existing@test.com",
            Password = "password123"
        };

        // Act
        var result = await _service.RegisterAsync(request);

        // Assert
        result.Success.Should().BeFalse();
    }
}
