﻿using Check_List_API.Data;
using Checklist_API.Features.JWT.Features.Interfaces;
using Checklist_API.Features.Users.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Checklist_API.Features.JWT.Features;

public class TokenGenerator(IConfiguration config, CheckListDbContext dbContext, ILogger<TokenGenerator> logger) : ITokenGenerator
{
    private readonly IConfiguration _config = config; 
    private readonly CheckListDbContext _dbContext = dbContext;
    private readonly ILogger<TokenGenerator> _logger = logger;

    public async Task<string> GenerateJSONWebTokenAsync(User user) 
    {
        _logger.LogInformation("Generating token");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = [];
        var userRoles = await _dbContext.JWTUserRole.Where(ur => ur.UserId == user.Id).ToListAsync();

        claims.Add(new Claim("UserId", user.Id.ToString()));
        claims.Add(new Claim("UserName", user.Email.ToString()));

        foreach (var role in userRoles) 
        {
            claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
        }

        var token = new JwtSecurityToken
            (
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(240),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
