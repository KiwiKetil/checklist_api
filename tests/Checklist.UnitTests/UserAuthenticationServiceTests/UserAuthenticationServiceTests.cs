using Checklist_API.Features.JWT.Features;
using Checklist_API.Features.Login.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Checklist.UnitTests.UserAuthenticationServiceTests;

public class UserAuthenticationServiceTests
{
    private readonly UserAuthenticationService _userAuthenticationService;
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ILogger<UserAuthenticationService>> _loggerMock = new();

    public UserAuthenticationServiceTests()
    {
        _userAuthenticationService = new UserAuthenticationService(_userRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AuthenticateUserAsync_WhenUserIsAuthenticated_ShouldReturnUser()
    {
        // Arrange
        var loginDTO = new LoginDTO { UserName = "ketilsveberg@gmail.com", Password = "string" };

        var expectedUser = new User
        {
            Id = UserId.NewId,
            FirstName = "Ketil",
            LastName = "Sveberg",
            Email = "ketilsveberg@gmail.com",
            HashedPassword = "$2a$11$J/m/v5v3hOVLKREX7jMZNO1xkMbtzU3vHf3Tm0Swc2MTszc0IpxO2",
            Salt = "$2a$11$55pfCgY8voiC1V4029QfR."
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(loginDTO.UserName)).ReturnsAsync(expectedUser);

        // Act
        var result = await _userAuthenticationService.AuthenticateUserAsync(loginDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.Id, result.Id);
        Assert.Equal(expectedUser.FirstName, result.FirstName);
        Assert.Equal(expectedUser.LastName, result.LastName);
        Assert.Equal(expectedUser.Email, result.Email);

        _userRepositoryMock.Verify(x => x.GetByEmailAsync(loginDTO.UserName), Times.Once);
    }

    [Fact]
    public async Task AuthenticateUserAsync_WhenValidUserUsesInvalidPassword_ShouldReturnNull()
    {
        // Arrange
        var loginDTO = new LoginDTO { UserName = "ketilsveberg@gmail.com", Password = "string" };

        var expectedUser = new User
        {
            Id = UserId.NewId,
            FirstName = "Ketil",
            LastName = "Sveberg",
            Email = "ketilsveberg@gmail.com",
            HashedPassword = "$2a$11$J/m/v5v3hOVLKREX7jMZNO1xkMbtzU3vHf3Tm0Swc2MTszc0Ip111",
            Salt = "$2a$11$55pfCgY8voiC1V4029QfR."
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(loginDTO.UserName)).ReturnsAsync(expectedUser);

        // Act
        var res = await _userAuthenticationService.AuthenticateUserAsync(loginDTO);

        // Assert
        Assert.Null(res);

        _userRepositoryMock.Verify(x => x.GetByEmailAsync(loginDTO.UserName), Times.Once);
    }

    [Fact]
    public async Task AuthenticateUserAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var loginDTO = new LoginDTO { UserName = "ketilsveberg@gmail.com", Password = "string" };
        User? nullUser = null;

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(loginDTO.UserName)).ReturnsAsync(nullUser);

        // Act
        var res = await _userAuthenticationService.AuthenticateUserAsync(loginDTO);

        // Assert
        Assert.Null(res);

        _userRepositoryMock.Verify(x => x.GetByEmailAsync(loginDTO.UserName), Times.Once);
    }
}
