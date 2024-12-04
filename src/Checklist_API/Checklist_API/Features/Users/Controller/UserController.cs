using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Checklist_API.Features.Users.Controller;
[Route("api/v1/users")]
[ApiController]
public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserController> _logger = logger;

    [Authorize(Roles = "Admin")]
    // GET https://localhost:7070/api/v1/users?page=1&pageSize=10 // logge hvem som henter users?? validuser eller anonymous
    [HttpGet(Name = "GetAllUsers")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers(int page = 1, int pageSize = 10)
    {
        var userId1 = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var username1 = User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
        var roles1 = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        if (page < 1 || pageSize < 1 || pageSize > 50)
        {
            _logger.LogDebug("Invalid pagination parameters Page: {page}, PageSize: {pageSize}", page, pageSize);
            return BadRequest("Invalid pagination parameters - MIN page = 1, MAX pageSize = 50 ");
        }

        var res = await _userService.GetAllUsersAsync(page, pageSize);
        return res != null ? Ok(res) : NotFound("No users found");
    }

    // GET https://localhost:7070/api/v1/users/ // logge hvem som henter validuser eller anonmymous
    [HttpGet("{id}", Name = "GetUserById")]
    public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] Guid id)
    {
        var res = await _userService.GetUserByIdAsync(id);
        return res != null ? Ok(res) : NotFound($"No user with ID {id} found");
    }

    // PUT https://localhost:7070/api/v1/users/
    [HttpPut("{id}", Name = "UpdateUser")]
    public async Task<ActionResult<UserDTO>> UpdateUser([FromRoute] Guid id, [FromBody] UserUpdateDTO dto)
    {
        var res = await _userService.UpdateUserAsync(id, dto);
        return res != null ? Ok(res) : NotFound($"No user with ID {id} found. Could not update user.");
    }

    // DELETE https://localhost:7070/api/v1/users/
    [HttpDelete("{id}", Name = "DeleteUser")]
    public async Task<ActionResult<UserDTO>> DeleteUser([FromRoute] Guid id)
    {
        var res = await _userService.DeleteUserAsync(id);
        return res != null ? Ok(res) : NotFound($"No user with ID {id} found. Could not delete user.");
    }

    // POST https://localhost:7070/api/v1/users/register // logge hva user registrerte seg som???
    [HttpPost("register", Name = "RegisterUser")]
    public async Task<ActionResult<UserDTO>> RegisterUser([FromBody] UserRegistrationDTO dto)
    {
        var res = await _userService.RegisterUserAsync(dto);
        return res != null ? Ok(res) : BadRequest("Could not register new user");
    }
}
