﻿using Checklist_API.Features.JWT.Features.Interfaces;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Checklist_API.Features.JWT.Features;

public class TokenGenerator(IConfiguration config, IUserRoleRepository UserRoleRepository, ILogger<TokenGenerator> logger) : ITokenGenerator
{
    private readonly IConfiguration _config = config; 
    private readonly IUserRoleRepository _userRoleRepository = UserRoleRepository;
    private readonly ILogger<TokenGenerator> _logger = logger;

    public async Task<string> GenerateJSONWebTokenAsync(User user) 
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        _logger.LogDebug("Generating token for user ID: {UserId}", user.Id);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var userRoles = await _userRoleRepository.GetUserRolesAsync(user.Id);

        List<Claim> claims = [];
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Email.ToString()));

        foreach (var role in userRoles) 
        {
            claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
        }

        var token = new JwtSecurityToken
            (
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(240),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
