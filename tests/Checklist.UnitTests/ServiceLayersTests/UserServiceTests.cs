﻿using Checklist_API.Features.Common.Interfaces;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Mappers;
using Checklist_API.Features.Users.Repository.Interfaces;
using Checklist_API.Features.Users.Service;
using Microsoft.Extensions.Logging;
using Moq;
using static Checklist_API.Features.ExceptionHandling.CustomExceptions;

namespace Checklist.UnitTests.ServiceLayersTests;
public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ILogger<UserService>> _loggerMock = new();
    private readonly IMapper<User, UserDTO> _userMapper;
    private readonly IMapper<User, UserUpdateDTO> _userUpdateMapper;
    private readonly IMapper<User, UserRegistrationDTO> _userRegistrationMapper;

    public UserServiceTests()
    {
        _userMapper = new UserMapper();
        _userUpdateMapper = new UserUpdateMapper();
        _userRegistrationMapper = new UserRegistrationMapper();
        _userService = new UserService(_userRepositoryMock.Object, _loggerMock.Object, _userMapper, _userUpdateMapper, _userRegistrationMapper);
    }

    #region GetallAsyncTests

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

        _userRepositoryMock.Setup(x => x.GetAllUsersAsync(page, pageSize)).ReturnsAsync(users);

        // Act
        var res = await _userService.GetAllUsersAsync(page, pageSize);

        // Assert
        Assert.NotNull(res);
        Assert.IsAssignableFrom<IEnumerable<UserDTO>>(res);
        Assert.Equal(users.Count(), res.Count());
    }

    [Fact]
    public async Task GetAllAsync_WhenThereAreNoUsers_ShouldReturnEmptyCollection()
    {
        //Arrange
        IEnumerable<User> users = [];

        int page = 1;
        int pageSize = 10;

        _userRepositoryMock.Setup(x => x.GetAllUsersAsync(page, pageSize)).ReturnsAsync(users);

        //Act
        var res = await _userService.GetAllUsersAsync(page, pageSize);

        //Assert
        Assert.NotNull(res);
        Assert.IsAssignableFrom<IEnumerable<UserDTO>>(res);
        Assert.Empty(res);
    }

    #endregion GetallAsyncTests

    #region GetByIdAsyncTests

    [Fact]
    public async Task GetUserByIdAsync_WhenRetrievingValidUser_ShouldReturnUserDTO()
    {
        // Arrange
        User user = new()
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
        };

        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(user.Id)).ReturnsAsync(user);

        // Act
        var res = await _userService.GetUserByIdAsync(user.Id.Value);

        // Assert   
        Assert.NotNull(res);
        Assert.IsType<UserDTO>(res);
        Assert.Equal(user.FirstName, res.FirstName);
        Assert.Equal(user.LastName, res.LastName);
        Assert.Equal(user.PhoneNumber, res.PhoneNumber);
        Assert.Equal(user.Email, res.Email);
        Assert.Equal(user.DateCreated, res.DateCreated);
        Assert.Equal(user.DateUpdated, res.DateUpdated);

        _userRepositoryMock.Verify(x => x.GetUserByIdAsync(user.Id), Times.Once);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUsingNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var id = UserId.NewId;

        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync((User?)null);

        // Act
        var res = await _userService.GetUserByIdAsync(id.Value);

        // Assert   
        Assert.Null(res);
        _userRepositoryMock.Verify(x => x.GetUserByIdAsync(id), Times.Once);
    }

    #endregion GetByIdAsyncTests

    #region UpdateUserTests

    //[Fact]
    //public async Task UpdateUser_WhenUpdatingUser_ShouldUpdateAndReturnUpdatedUser()
    //{
    //    // Arrange

    //    User user = new()
    //    {
    //        Id = UserId.NewId,
    //        FirstName = "Nils",
    //        LastName = "Jensen",
    //        PhoneNumber = "83542435",
    //        Email = "jensen@gmail.com",
    //        HashedPassword = "hashedPassword",
    //        Salt = "salt",
    //        DateCreated = new DateTime(2024, 11, 17, 02, 50, 00),
    //        DateUpdated = new DateTime(2024, 12, 17, 02, 52, 30)
    //    };

    //    // Act

    //    // Assert

    //}

    #endregion UpdateUserTests

    #region RegisterUserAsync

    [Fact]
    public async Task RegisterUserAsync_WhenUserRegistersWithSuccess_ShouldReturnUserDTO()
    {
        // Arrange
        UserRegistrationDTO dto = new("Nils", "Jensen", "83542435", "jensen@gmail.com", "fakePassword");

        User user = new()
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
        };

        _userRepositoryMock.Setup(x => x.GetUserByEmailAsync(dto.Email)).ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(x => x.RegisterUserAsync(It.IsAny<User>())).ReturnsAsync(user);

        // Act
        var res = await _userService.RegisterUserAsync(dto);

        // Assert
        Assert.NotNull(res);
        Assert.IsType<UserDTO>(res);

        Assert.Equal(user.FirstName, res.FirstName);
        Assert.Equal(user.LastName, res.LastName);
        Assert.Equal(user.PhoneNumber, res.PhoneNumber);
        Assert.Equal(user.Email, res.Email);
        Assert.Equal(user.DateCreated, res.DateCreated);
        Assert.Equal(user.DateUpdated, res.DateUpdated);

        _userRepositoryMock.Verify(x => x.GetUserByEmailAsync(dto.Email), Times.Once);
        _userRepositoryMock.Verify(x => x.RegisterUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_WhenUserAlreadyExists_ShouldThrowUserAlreadyExistException()
    {
        // Arrange
        UserRegistrationDTO dto = new("Nils", "Jensen", "83542435", "jensen@gmail.com", "fakePassword");

        User user = new()
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
        };

        _userRepositoryMock.Setup(x => x.GetUserByEmailAsync(dto.Email)).ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _userService.RegisterUserAsync(dto));
    }

    #endregion RegisterUserAsync
}
