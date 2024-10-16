using Checklist_API.Features.JWT.Features;
using Checklist_API.Features.Login.Controller;
using Checklist_API.Features.Login.DTOs;
using Checklist_API.Features.Users.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Checklist_API.Features.JWT.Features.Interfaces;

namespace Checklist.UnitTests.Controllers
{
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

        [Fact]
        public async Task Login_WhenSuccess_ShouldReturnJwtToken()
        {
            // Arrange
            var loginDTO = new LoginDTO { UserName = "ketilSveberg#", Password = "S1eberg#" };
            var user = new User { Id = UserId.NewId, FirstName = "Ketil", LastName = "Sveberg", Email = "ketilsveberg@gmail.com", HashedPassword = "hashedPassword" };
            var tokenString = "diddydidit";

            // Set up mocks to return expected values
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
    }
}
