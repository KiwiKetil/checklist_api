using Checklist_API.Features.Users.Controller;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Checklist.UnitTests.ControllersTests;
public class UserControllerTests
{
    private readonly UserController _userController;
    private readonly Mock<ClaimsPrincipal> _mockUser;
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<ILogger<UserController>> _loggerMock = new();

    public UserControllerTests()
    {
        _mockUser = new Mock<ClaimsPrincipal>();
        _mockUser.Setup(u => u.Claims).Returns(new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, "894efa5f-d594-4064-9200-fad11766bd83")
        });

        var controllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = _mockUser.Object }
        };

        _userController = new(_userServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = controllerContext
        };
    }

    #region GetAllUsersTests

    [Theory]
    [InlineData(1, 10)]
    [InlineData(1, 5)]

    public async Task GetAllUsers_WithPagingValues_ShouldReturnOKAndAllUsers(int page, int pageSize)
    {
        // Arrange

        List<UserDTO> dtos =
        [
            new UserDTO("Ketil", "Sveberg", "12345678", "Sveberg@gmail.com", new DateTime(2024, 10, 17, 02, 50, 00), new DateTime(2024, 10, 17, 02, 52, 30)),
            new UserDTO("Quyen", "Ho", "23456789", "quyen99@gmail.com", new DateTime(2024, 10, 18, 02, 51, 00), new DateTime(2024, 10, 17, 03, 55, 40)),
            new UserDTO("Nico", "Ho", "12345678", "nico@gmail.com", new DateTime(2024, 10, 19, 02, 52, 00), new DateTime(2024, 10, 17, 12, 00, 45))
        ];

        _userServiceMock.Setup(x => x.GetAllUsersAsync(page, pageSize)).ReturnsAsync(dtos);

        // Act
        var res = await _userController.GetAllUsers(page, pageSize);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(res); // This checks that res is of type ActionResult<IEnumerable<UserDTO>>.
        var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result); // This asserts that the Result inside actionResult is of type OkObjectResult.
        var dtoCollection = Assert.IsType<List<UserDTO>>(returnValue.Value); // This asserts that the Value inside the OkObjectResult is a List<UserDTO>, AND IT CONTAINS ALL THE DATA.

        Assert.Equal(dtos.Count, dtoCollection.Count);

        foreach (var (expected, actual) in dtos.Zip(dtoCollection, (expected, actual) => (expected, actual)))
        {
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.PhoneNumber, actual.PhoneNumber);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.DateCreated, actual.DateCreated);
            Assert.Equal(expected.DateUpdated, actual.DateUpdated);
        }

        _userServiceMock.Verify(x => x.GetAllUsersAsync(page, pageSize), Times.Once);
    }


    [Theory]
    [InlineData(1, 10)]
    [InlineData(1, 5)]

    public async Task GetAllUsers_WhenNoUsersFound_WithPagingValues_ShouldReturnNotFound(int page, int pageSize)
    {
        // Arrange
        var mockUser = new Mock<ClaimsPrincipal>();
        mockUser.Setup(u => u.Claims).Returns(new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, "fakeUserId123"),
            new(JwtRegisteredClaimNames.Name, "fakeUserName"),
            new(ClaimTypes.Role, "Admin"),
        });

        var controllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = mockUser.Object }
        };
        _userController.ControllerContext = controllerContext;

        _userServiceMock.Setup(x => x.GetAllUsersAsync(page, pageSize)).ReturnsAsync(() => null!);

        // Act
        var res = await _userController.GetAllUsers(page, pageSize);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(res); // This checks that res is of type ActionResult<IEnumerable<UserDTO>>.
        var returnValue = Assert.IsType<NotFoundObjectResult>(actionResult.Result); // This asserts that the Result inside actionResult is of type NotFoundObjectResult.
        var errorMessage = Assert.IsType<string>(returnValue.Value); // This asserts that the Value inside the OkObjectResult is a List<UserDTO>, AND IT CONTAINS ALL THE DATA.
        Assert.Equal("No users found", errorMessage);

        _userServiceMock.Verify(x => x.GetAllUsersAsync(page, pageSize), Times.Once);
    }

    #endregion GetAllUsersTests

    #region GetUserByIdTests

    [Fact]
    public async Task GetUserById_WhenRetrievingValidUser_ShouldReturnUserDTO()
    {
        // Arrange
        Guid id = new("894efa5f-d594-4064-9200-fad11766bd83");

        UserDTO userDTO = new(
            "Ketil",
            "Sveberg",
            "12345678",
            "Sveberg@gmail.com",
            new DateTime(2024, 10, 17, 02, 50, 00),
            new DateTime(2024, 10, 17, 02, 52, 30));

        _userServiceMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync(userDTO);
        _mockUser.Setup(u => u.FindFirst(JwtRegisteredClaimNames.Sub)).Returns(new Claim(JwtRegisteredClaimNames.Sub, "894efa5f-d594-4064-9200-fad11766bd83"));

        // Act
        var res = await _userController.GetUserById(id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDTO>>(res);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dto = Assert.IsType<UserDTO>(okResult.Value);

        Assert.Equal(dto.FirstName, userDTO.FirstName);
        Assert.Equal(dto.LastName, userDTO.LastName);
        Assert.Equal(dto.PhoneNumber, userDTO.PhoneNumber);
        Assert.Equal(dto.Email, userDTO.Email);
        Assert.Equal(dto.DateCreated, userDTO.DateCreated);
        Assert.Equal(dto.DateUpdated, userDTO.DateUpdated);

        _userServiceMock.Verify(x => x.GetUserByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetUserById_WhenRetrievingUserThatDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        Guid id = new("6ec1e5b9-4206-41d5-8888-b4771bf9d9c1");

        _userServiceMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync((UserDTO?)null);
        _mockUser.Setup(u => u.FindFirst(JwtRegisteredClaimNames.Sub)).Returns(new Claim(JwtRegisteredClaimNames.Sub, "6ec1e5b9-4206-41d5-8888-b4771bf9d9c1"));

        // Act
        var res = await _userController.GetUserById(id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDTO>>(res);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal($"No user with ID {id} found", notFoundResult.Value);

        _userServiceMock.Verify(x => x.GetUserByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetUserById_WhenRetrievingUserWhenIsNotAuthorized_ShouldReturnUnAuthorized()
    {
        // Arrange
        Guid id = new("345afc12-905c-40b2-b79b-6a98df2f9c72");

        _userServiceMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync((UserDTO?)null);
        _mockUser.Setup(u => u.FindFirst(JwtRegisteredClaimNames.Sub)).Returns(new Claim(JwtRegisteredClaimNames.Sub, "894efa5f-d594-4064-9200-fad11766bd83"));

        // Act
        var res = await _userController.GetUserById(id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDTO>>(res);
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);
        Assert.Equal("Not authorized to get this user", unauthorizedResult.Value);

        _userServiceMock.Verify(x => x.GetUserByIdAsync(id), Times.Never);
    }

    [Fact]
    public async Task GetUserById_WhenRetrievingUserWhenIsAdmin_ShouldReturnUserDTO()
    {
        // Arrange
        Guid id = new("894efa5f-d594-4064-9200-fad11766bd83");
        UserDTO userDTO = new(
        "Ketil",
        "Sveberg",
        "12345678",
        "Sveberg@gmail.com",
        new DateTime(2024, 10, 17, 02, 50, 00),
        new DateTime(2024, 10, 17, 02, 52, 30));

        _userServiceMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync(userDTO);

        var claims = new List<Claim>
        {
        new Claim(JwtRegisteredClaimNames.Sub, "ae045d60-8b4e-4b5e-b229-6a8c8a97bcfd"),
        new Claim(ClaimTypes.Role, "Admin")
        };

        _userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims))
            }
        };

        _mockUser.Setup(u => u.Claims).Returns(claims);

        // Act
        var res = await _userController.GetUserById(id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDTO>>(res);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dto = Assert.IsType<UserDTO>(okResult.Value);

        Assert.Equal(dto.FirstName, userDTO.FirstName);
        Assert.Equal(dto.LastName, userDTO.LastName);
        Assert.Equal(dto.PhoneNumber, userDTO.PhoneNumber);
        Assert.Equal(dto.Email, userDTO.Email);
        Assert.Equal(dto.DateCreated, userDTO.DateCreated);
        Assert.Equal(dto.DateUpdated, userDTO.DateUpdated);

        _userServiceMock.Verify(x => x.GetUserByIdAsync(id), Times.Once);
    }

    #endregion GetUserByIdTests

    #region UpdateUserTests
     
    [Fact]
    public async Task UpdateUser_WhenUpdatingUserWithValidUserID_ShouldReturnUpdatedUserDTO() 
    {
        // Arrange

        Guid id = new("6ec1e5b9-4206-41d5-8888-b4771bf9d9a2");

        UserUpdateDTO updateDTO = new
        (
            "Ketil",
            "Sveberg",
            "71717171",
            "ks@gmail.com"
        );

        UserDTO userDTO = new
        (
            "Ketil",
            "Sveberg",
            "71717171",
            "ks@gmail.com",
            DateTime.Now,
            DateTime.Now
        );

        _userServiceMock.Setup(x => x.UpdateUserAsync(id, updateDTO)).ReturnsAsync(userDTO);
        _mockUser.Setup(u => u.FindFirst(JwtRegisteredClaimNames.Sub)).Returns(new Claim(JwtRegisteredClaimNames.Sub, "6ec1e5b9-4206-41d5-8888-b4771bf9d9a2"));

        // Act

        var res = await _userController.UpdateUser(id, updateDTO);

        // Assert

        var actionResult = Assert.IsType<ActionResult<UserDTO>>(res);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dto = Assert.IsType<UserDTO>(okResult.Value);

        Assert.NotNull(res);
        Assert.Equal(updateDTO.FirstName, userDTO.FirstName);
        Assert.Equal(updateDTO.LastName, userDTO.LastName);
        Assert.Equal(updateDTO.PhoneNumber, userDTO.PhoneNumber);
        Assert.Equal(updateDTO.Email, userDTO.Email);

        _userServiceMock.Verify(x => x.UpdateUserAsync(id, updateDTO), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_WhenUpdatingUserButUserIsNotAuthorized_ShouldReturnUnauthorized()
    {
        // Arrange

        Guid id = new("6ec1e5b9-4206-41d5-8888-b4771bf9d9c3");

        UserUpdateDTO updateDTO = new
        (
            "Ketil",
            "Sveberg",
            "71717171",
            "ks@gmail.com"
        );

        _mockUser.Setup(u => u.FindFirst(JwtRegisteredClaimNames.Sub)).Returns(new Claim(JwtRegisteredClaimNames.Sub, "6ec1e5b9-4206-41d5-8888-b4771bf9d9a2"));

        // Act

        var res = await _userController.UpdateUser(id, updateDTO);

        // Assert

        var actionResult = Assert.IsType<ActionResult<UserDTO>>(res);
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);
        Assert.Equal("Not authorized to update this user", unauthorizedResult.Value);

        _userServiceMock.Verify(x => x.UpdateUserAsync(id, updateDTO), Times.Never);
    }

    // when user not foundtest

    // AsAdmin test should return ok

    #endregion UpdateUsertests

    #region RegisterUserTests    

    #region using TheoryData V1 (also contains the Fact test for Badrequest.) 

    public static TheoryData<UserRegistrationDTO, UserDTO> GetUserRegistrationDTOsWithExpectedResults()
    {
        var testData = new TheoryData<UserRegistrationDTO, UserDTO>
        {
            {
                new UserRegistrationDTO("Ketil", "Sveberg", "12345678", "Sveberg@gmail.com", "password"),
                new UserDTO("Ketil", "Sveberg", "12345678", "Sveberg@gmail.com", DateTime.UtcNow, DateTime.UtcNow)
            },
            {
                new UserRegistrationDTO("Quyen", "Ho", "42534253", "Quyen99@gmail.com", "password2"),
                new UserDTO("Quyen", "Ho", "42534253", "Quyen99@gmail.com", DateTime.UtcNow, DateTime.UtcNow)
            },
            {
                new UserRegistrationDTO("Nico", "Ho", "42534253", "Nico@gmail.com", "password3"),
                new UserDTO("Nico", "Ho", "42534253", "Nico@gmail.com", DateTime.UtcNow, DateTime.UtcNow)
            }
        };

        return testData;
    }

    [Theory]
    [MemberData(nameof(GetUserRegistrationDTOsWithExpectedResults))] // kunne brukt ClassDtaa or InlineData for å ikke ha warning, lar være her pga DTOs er serializable

    public async Task RegisterUser_WhenUserRegistersWithSuccess_ShouldReturnOKAndUserDTO(UserRegistrationDTO dto, UserDTO expectedUserDTO)
    {
        // Arrange
        _userServiceMock.Setup(x => x.RegisterUserAsync(dto)).ReturnsAsync(expectedUserDTO);

        // Act
        var result = await _userController.RegisterUser(dto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedDTO = Assert.IsType<UserDTO>(okResult.Value);
        Assert.Equal(expectedUserDTO, returnedDTO);

        _userServiceMock.Verify(x => x.RegisterUserAsync(dto), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_WhenUserRegistrationFails_ShouldReturnBadRequest400()
    {
        //Arrange
        UserRegistrationDTO dto = new("Nico", "Ho", "42534253", "Nico@gmail.com", "password3");

        _userServiceMock.Setup(x => x.RegisterUserAsync(dto)).ReturnsAsync((UserDTO?)null);

        //Act
        var res = await _userController.RegisterUser(dto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDTO>>(res);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

        _userServiceMock.Verify(x => x.RegisterUserAsync(dto), Times.Once);
    }

    #endregion using TheoryData V1 (also contains the Fact test for Badrequest.) 

    #region using TheoryData V2  

    public static TheoryData<UserRegistrationDTO> GetUserRegistrationDTOs()
    {
        return
        [
        new("Ketil", "Sveberg", "12345678", "Sveberg@gmail.com", "password"),
        new("Quyen", "Ho", "42534253", "Quyen99@gmail.com", "password2"),
        new("Nico", "Ho", "42534253", "Nico@gmail.com", "password3")
        ];
    }

    [Theory]
    [MemberData(nameof(GetUserRegistrationDTOs))] // warning i tilfelle ikke er serializable: ikke primitive datatyper i DTO.

    public async Task RegisterUser_WhenUserRegistersWithSuccess_ShouldReturnOKAndUserDTOV2(UserRegistrationDTO dto)
    {
        // Arrange
        var dtNow = DateTime.UtcNow;

        var expectedUserDTO = new UserDTO(
            dto.FirstName,
            dto.LastName,
            dto.PhoneNumber,
            dto.Email,
            dtNow,
            dtNow);

        _userServiceMock.Setup(x => x.RegisterUserAsync(dto)).ReturnsAsync(expectedUserDTO);

        // Act
        var result = await _userController.RegisterUser(dto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
        var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedDTO = Assert.IsType<UserDTO>(returnValue.Value);
        Assert.Equal(expectedUserDTO, returnedDTO);

        _userServiceMock.Verify(x => x.RegisterUserAsync(dto), Times.Once);
    }
    #endregion using TheoryData V2

    #region using InlineData V3

    [Theory]
    [InlineData("Ketil", "Sveberg", "12345678", "Sveberg@gmail.com", "password")]
    [InlineData("Quyen", "Ho", "42534253", "Quyen99@gmail.com", "password2")]
    [InlineData("Nico", "Ho", "42534253", "Nico@gmail.com", "password3")]

    public async Task RegisterUser_WhenUserRegistersWithSuccess_ShouldReturnOKAndUserDTOV3(string firstName, string lastName, string phoneNumber, string email, string password)
    {
        // Arrange
        var userRegistrationDTO = new UserRegistrationDTO(firstName, lastName, phoneNumber, email, password);

        _userServiceMock.Setup(x => x.RegisterUserAsync(userRegistrationDTO))
                    .ReturnsAsync(new UserDTO(
                        userRegistrationDTO.FirstName,
                        userRegistrationDTO.LastName,
                        userRegistrationDTO.PhoneNumber,
                        userRegistrationDTO.Email,
                        DateTime.UtcNow,
                        DateTime.UtcNow)
                    );

        // Act
        var res = await _userController.RegisterUser(userRegistrationDTO);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDTO>>(res);
        var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedDTO = Assert.IsType<UserDTO>(returnValue.Value);

        Assert.Equal(userRegistrationDTO.FirstName, returnedDTO.FirstName);
        Assert.Equal(userRegistrationDTO.LastName, returnedDTO.LastName);
        Assert.Equal(userRegistrationDTO.PhoneNumber, returnedDTO.PhoneNumber);
        Assert.Equal(userRegistrationDTO.Email, returnedDTO.Email);

        Assert.True(returnedDTO.DateCreated <= DateTime.UtcNow);
        Assert.True(returnedDTO.DateUpdated <= DateTime.UtcNow);

        _userServiceMock.Verify(x => x.RegisterUserAsync(userRegistrationDTO), Times.Once);
    }

    #endregion using InlineData V3

    #region using ClassData V4

    // ikke implementert. Er bare en annen måte å gjøre Theorydata på men med bruks av egen klasse (arv fra IEnumerable<object[]>) for testdata.

    #endregion using ClassData V4

    #endregion RegisterUserTests
}
