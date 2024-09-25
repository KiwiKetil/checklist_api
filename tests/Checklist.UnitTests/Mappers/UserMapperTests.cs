﻿using Checklist_API.Features.Common.Interfaces;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklist.UnitTests.Mappers;

public class UserMapperTests
{
    private readonly IMapper<User, UserDTO> _userMapper = new UserMapper();

    [Fact]  

    public void MapToDTO_WhenMappingUserToUserDTO_ShouldReturnUserDTO()
    {
        // Act

        User user = new User()
        {
            Id = UserId.NewId,
            FirstName = "Ketil",
            LastName = "Sveberg",
            PhoneNumber = "11112222",
            Email = "Ketilsveberg@gmail.com",
            HashedPassword = "$2a$11$6LeztI8J.RjEFt62ctA8a.yyM1V6X3bOEe6Rv5we2iPIe9namsjU6",
            Salt = "$2a$11$9p6QfwD5gdrrg4Mvzf/Mgu",
            DateCreated = new DateTime(2023, 12, 13, 11, 00, 00),
            DateUpdated = new DateTime(2023, 12, 13, 11, 00, 00),
        };

        // Arrange

        var userDTO = _userMapper.MapToDTO(user);

        // Assert

        Assert.NotNull(userDTO);
        Assert.Equal(user.FirstName, userDTO.FirstName);
        Assert.Equal(user.LastName, userDTO.LastName);
        Assert.Equal(user.PhoneNumber, userDTO.PhoneNumber);
        Assert.Equal(user.Email, userDTO.Email);
        Assert.Equal(user.DateCreated, userDTO.DateCreated);
        Assert.Equal(user.DateUpdated, userDTO.DateUpdated);
    }

    [Fact]

    public void MapToUser_WhenMappingUserDTOToUser_ShouldReturnUser()
    {
        // Act

        UserDTO userDTO = new
            (
            "Ketil",
            "Sveberg",
            "11112222",
            "Ketilsveberg@gmail.com",
            new DateTime(2023, 12, 13, 11, 00, 00),
            new DateTime(2023, 12, 13, 11, 00, 00)
            );

        // Arrange

        var user = _userMapper.MapToEntity(userDTO);
        
        // Assert

        Assert.NotNull(user);
        Assert.Equal(userDTO.FirstName, user.FirstName);
        Assert.Equal(userDTO.LastName, user.LastName);
        Assert.Equal(userDTO.PhoneNumber, user.PhoneNumber);
        Assert.Equal(userDTO.Email, user.Email);
        //Assert.Equal(userDTO.DateCreated, user.DateCreated);
        //Assert.Equal(userDTO.DateUpdated, user.DateUpdated);
    }
}
