using Castle.Core.Logging;
using Checklist_API.Features.Users.Controller;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist.UnitTests.Controllers;
public class UserControllerTests
{
    private readonly UserController _userController;
    private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
    private readonly Mock<ILogger<UserController>> _loggerMock = new Mock<ILogger<UserController>>();

    public UserControllerTests()
    {
        _userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
    }

    [Theory]
    [InlineData(1, 10)]

    public async Task GetAllUsersAsync_ShouldReturn_AllUsers_WithPagingValues(int page, int pageSize)
    {
        // arrange

        List<UserDTO> dtos = new()
        {
            new UserDTO("Ketil", "Sveberg", "12345678", "Sveberg@.gmail.com", new DateTime(2024, 10, 17, 02, 50, 00), new DateTime(2024, 10, 17, 02, 52, 30)),
            new UserDTO("Quyen", "Ho", "23456789", "quyen99@gmail.com", new DateTime(2024, 10, 18, 02, 51, 00), new DateTime(2024, 10, 17, 03, 55, 40)),
            new UserDTO("Nico", "Ho", "12345678", "nico@gmail.com", new DateTime(2024, 10, 19, 02, 52, 00), new DateTime(2024, 10, 17, 12, 00, 45))
        };

        _userServiceMock.Setup(x => x.GetAllAsync(page, pageSize)).ReturnsAsync(dtos);

        // Act

        var res = await _userController.GetAll();

        // Assert


    }
}
