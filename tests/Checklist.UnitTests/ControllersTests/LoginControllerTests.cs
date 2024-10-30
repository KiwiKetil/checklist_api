using Checklist_API.Features.JWT.Features;
using Checklist_API.Features.Login.Controller;
using Checklist_API.Features.Login.DTOs;
using Checklist_API.Features.Users.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Checklist_API.Features.JWT.Features.Interfaces;

namespace Checklist.UnitTests.ControllersTests;

public class LoginControllerTests
{
    private readonly LoginController _loginController;
    private readonly Mock<IUserAuthenticationService> _authServiceMock = new(); 
    private readonly Mock<ITokenGenerator> _tokenGeneratorMock = new();
    private readonly Mock<ILogger<LoginController>> _controllerLoggerMock = new();

    public LoginControllerTests()
    {
        _loginController = new LoginController(_authServiceMock.Object, _tokenGeneratorMock.Object, _controllerLoggerMock.Object);
    }

    #region SuccessfulLogin

    [Fact]
    public async Task Login_WhenSuccess_ShouldReturnJwtToken()
    {
        // Arrange
        var loginDTO = new LoginDTO { UserName = "ketilSveberg#", Password = "S1eberg#" };
        var user = new User { Id = UserId.NewId, FirstName = "Ketil", LastName = "Sveberg", Email = "ketilsveberg@gmail.com", HashedPassword = "hashedPassword" };
        var tokenString = "diddydidit";

        _authServiceMock.Setup(x => x.AuthenticateUserAsync(loginDTO)).ReturnsAsync(user);
        _tokenGeneratorMock.Setup(x => x.GenerateJSONWebTokenAsync(user)).ReturnsAsync(tokenString);

        // Act
        var result = await _loginController.Login(loginDTO);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;

        var actualResult = okResult!.Value as TokenResponse;
        Assert.NotNull(actualResult);
        Assert.Equal(tokenString, actualResult.Token);

        _authServiceMock.Verify(x => x.AuthenticateUserAsync(loginDTO), Times.Once);
        _tokenGeneratorMock.Verify(x => x.GenerateJSONWebTokenAsync(user), Times.Once);
    }

    #endregion SuccessfulLogin

    #region InvalidPasswordLoginFail

    [Fact]
    public async Task Login_WhenUsingInvalidPassword_ShouldNotReturnToken()
    {
        // Arrange
        var InvalidloginDTO = new LoginDTO { UserName = "ketilSveberg#", Password = "S1eberg#" };
        User? user = null;
        TokenResponse InvalidToken = new() { Token = "abcde" };

        _authServiceMock.Setup(x => x.AuthenticateUserAsync(InvalidloginDTO)).ReturnsAsync(user);

        // Act
        var result = await _loginController.Login(InvalidloginDTO);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<UnauthorizedObjectResult>(result);
        var unauthorizedResult = result as UnauthorizedObjectResult;
        Assert.Equal("Not Authorized", unauthorizedResult!.Value); 

        _authServiceMock.Verify(x => x.AuthenticateUserAsync(InvalidloginDTO), Times.Once);
        _tokenGeneratorMock.Verify(x => x.GenerateJSONWebTokenAsync(It.IsAny<User>()), Times.Never);
    }

    #endregion InvalidPasswordLoginFail

}
