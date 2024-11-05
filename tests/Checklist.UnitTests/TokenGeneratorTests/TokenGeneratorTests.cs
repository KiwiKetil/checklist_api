using Microsoft.Extensions.Configuration;
using Checklist_API.Features.JWT.Features;
using Checklist_API.Features.JWT.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.JWT.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;

namespace Checklist.UnitTests.TokenGeneratorTests;
public class TokenGeneratorTests
{
    private readonly TokenGenerator _tokenGenerator;
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly Mock<IUserRoleRepository> _userRoleRepositoryMock = new();
    private readonly Mock<ILogger<TokenGenerator>> _loggerMock = new();

    public TokenGeneratorTests()
    {
        _tokenGenerator = new(_configMock.Object, _userRoleRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GenerateJSONWebTokenAsync_WhenUserSuccesfullyLogsIn_ShouldGenerateAndReturnJwtToken_WithCorrectClaims()
    {
        // Arrange 
        User user = new()
        {
            Id = UserId.NewId,
            FirstName = "Ketil",
            LastName = "Sveberg",
            Email = "ketilsveberg@gmail.com",
            HashedPassword = "$2a$11$J/m/v5v3hOVLKREX7jMZNO1xkMbtzU3vHf3Tm0Swc2MTszc0IpxO2",
            Salt = "$2a$11$55pfCgY8voiC1V4029QfR."
        };

        _configMock.Setup(x => x["Jwt:Key"]).Returns("ThisismySecretKeyDoNotStoreHereForGodsSake");
        _configMock.Setup(x => x["Jwt:Issuer"]).Returns("Checklist_API");
        _configMock.Setup(x => x["Jwt:Audience"]).Returns("Checklist_API");


        List<UserRole> userRoles =
        [
            new UserRole  { RoleName = "Admin"},
            new UserRole  { RoleName = "User"}
        ];

        _userRoleRepositoryMock.Setup(x => x.GetUserRolesAsync(user.Id)).ReturnsAsync(userRoles);

        // Act
        var res = await _tokenGenerator.GenerateJSONWebTokenAsync(user);

        // Assert
        Assert.NotNull(res);
        Assert.IsType<string>(res);

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(res);

        Assert.Equal(user.Id.ToString(), jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
        Assert.Equal(user.Email, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Name).Value);
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
        Assert.Equal("Checklist_API", jwtToken.Issuer);
        Assert.Contains("Checklist_API", jwtToken.Audiences);
    }

    [Fact]
    public async Task GenerateJSONWebTokenAsync_WhenUserIsNull_ShouldThrowArgumentNullException()
    {
       // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _tokenGenerator.GenerateJSONWebTokenAsync(null!));
    }

    [Fact]
    public async Task GenerateJSONWebTokenAsync_ShouldExpireAfterSetDuration()
    {
        // Arrange
        User user = new()
        {
            Id = UserId.NewId,
            FirstName = "Ketil",
            LastName = "Sveberg",
            Email = "ketilsveberg@gmail.com",
            HashedPassword = "$2a$11$J/m/v5v3hOVLKREX7jMZNO1xkMbtzU3vHf3Tm0Swc2MTszc0IpxO2",
            Salt = "$2a$11$55pfCgY8voiC1V4029QfR."
        };

        _configMock.Setup(x => x["Jwt:Key"]).Returns("ThisismySecretKeyDoNotStoreHereForGodsSake");
        _configMock.Setup(x => x["Jwt:Issuer"]).Returns("Checklist_API");
        _configMock.Setup(x => x["Jwt:Audience"]).Returns("Checklist_API");

        var futureDateTime = DateTime.UtcNow.AddHours(4).AddMinutes(1);

        // Act
        var token = await _tokenGenerator.GenerateJSONWebTokenAsync(user);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var expirationClaim = jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value;
        var expirationDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim)).UtcDateTime;

        Assert.True(futureDateTime > expirationDate, "Token should be expired");
    }

    [Fact]
    public async Task GenerateJSONWebTokenAsync_UpToExpirationtime_ShouldStillBeValid()
    {
        // Arrange

        User user = new()
        {
            Id = UserId.NewId,
            FirstName = "Ketil",
            LastName = "Sveberg",
            Email = "ketilsveberg@gmail.com",
            HashedPassword = "$2a$11$J/m/v5v3hOVLKREX7jMZNO1xkMbtzU3vHf3Tm0Swc2MTszc0IpxO2",
            Salt = "$2a$11$55pfCgY8voiC1V4029QfR."
        };

        _configMock.Setup(x => x["Jwt:Key"]).Returns("ThisismySecretKeyDoNotStoreHereForGodsSake");
        _configMock.Setup(x => x["Jwt:Issuer"]).Returns("Checklist_API");
        _configMock.Setup(x => x["Jwt:Audience"]).Returns("Checklist_API");

        // Act
        var res = await _tokenGenerator.GenerateJSONWebTokenAsync(user);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(res);

        var expirationClaim = jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value;
        var expirationDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim)).DateTime;

        Assert.True(expirationDate > DateTime.UtcNow, "Token should not be expired");
    }    
}