﻿using Check_List_API.Data;
using Checklist_API.Features.JWT.Features.Interfaces;
using Checklist_API.Features.Login.DTOs;
using Checklist_API.Features.Users.Entity;
using Microsoft.EntityFrameworkCore;

namespace Checklist_API.Features.JWT.Features;

public class UserAuthenticationService(CheckListDbContext dbContext, ILogger<UserAuthenticationService> logger) : IUserAuthenticationService
{
    private readonly CheckListDbContext _dbContext = dbContext;
    private readonly ILogger<UserAuthenticationService> _logger = logger;


    public async Task<User?> AuthenticateUserAsync(LoginDTO loginDTO) 
    {
        _logger.LogInformation("Authenticating user: {username}", loginDTO.UserName);

        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == loginDTO.UserName);

        if (user != null && BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.HashedPassword)) 
        {
            return user;
        }

        return null;
    }     
}
