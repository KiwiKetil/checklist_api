﻿using Checklist_API.Features.JWT.Features.Interfaces;
using Checklist_API.Features.Login.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Checklist_API.Features.JWT.Features;

public class UserAuthenticationService(IUserRepository userRepository, ILogger<UserAuthenticationService> logger) : IUserAuthenticationService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<UserAuthenticationService> _logger = logger;

    public async Task<User?> AuthenticateUserAsync(LoginDTO loginDTO) 
    {
        _logger.LogInformation("Authenticating user: {username}", loginDTO.UserName);

        var user = await _userRepository.GetByEmailAsync(loginDTO.UserName);

        if (user != null && BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.HashedPassword)) 
        {
            return user;
        }

        return null;
    }     
}
