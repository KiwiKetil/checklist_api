using Checklist_API.Features.Common.Interfaces;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Mappers;
using Checklist_API.Features.Users.Repository.Interfaces;
using Checklist_API.Features.Users.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace Checklist.UnitTests.ServiceLayersTests;
public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ILogger<UserService>> _loggerMock = new();
    private readonly IMapper<User, UserDTO> _userMapper;
    private readonly Mock<IMapper<User, UserRegistrationDTO>> _userRegistrationMapperMock = new();

    public UserServiceTests()
    {
        _userMapper = new UserMapper();
        _userService = new UserService(_userRepositoryMock.Object, _loggerMock.Object, _userMapper, _userRegistrationMapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_WhenRetrievingAllUsers_ShouldReturnAllUsers()
    {
        // Arrange
        IEnumerable<User> users =
        [
            new User
            {
                Id = UserId.NewId,
                FirstName = "Jimmy",
                LastName = "Stallone",
                PhoneNumber = "73463577",
                Email = "stallone@gmail.com",
                HashedPassword = "hashedPassword",
                Salt = "salt",
                DateCreated = new DateTime(2024, 11, 17, 02, 50, 00),
                DateUpdated = new DateTime(2024, 12, 17, 02, 52, 30)
            },

            new User
            {
                Id = UserId.NewId,
                FirstName = "Sarah",
                LastName = "Connor",
                PhoneNumber = "19197676",
                Email = "sarah.connor@gmail.com",
                HashedPassword = "hashedPassword",
                Salt = "salt",
                DateCreated = new DateTime(2024, 11, 17, 02, 50, 00),
                DateUpdated = new DateTime(2024, 12, 17, 02, 52, 30)
            },

            new User
            {
                Id = UserId.NewId,
                FirstName = "Nils",
                LastName = "Jensen",
                PhoneNumber = "83542435",
                Email = "jensen@gmail.com",
                HashedPassword = "hashedPassword",
                Salt = "salt",
                DateCreated = new DateTime(2024, 11, 17, 02, 50, 00),
                DateUpdated = new DateTime(2024, 12, 17, 02, 52, 30)
            },
        ];

        int page = 1;
        int pageSize = 10;

        _userRepositoryMock.Setup(x => x.GetAllAsync(page, pageSize)).ReturnsAsync(users);   

        // Act
        var res = await _userService.GetAllAsync(page, pageSize);

        // Assert
        Assert.NotNull(res);
        Assert.IsAssignableFrom<IEnumerable<UserDTO>>(res);
        Assert.Equal(res.Count(), users.Count());      
    }

    [Fact]
    public async Task GetAllAsync_WhenThereAreNoUsers_ShouldReturnEmptyCollection() 
    {
        await Task.Delay(10);
    
    }
}
