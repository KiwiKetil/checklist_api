using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Checklist_API.Features.Users.Controller;
[Route("api/v1/users")]
[ApiController]
public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserController> _logger = logger;

    [Authorize(Roles = "User")]
    // GET https://localhost:7070/api/v1/users?page=1&pageSize=10
    [HttpGet(Name = "GetAllUsers")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers(int page = 1, int pageSize = 10)
    {
        _logger.LogInformation("Retrieving all users");

        if (page < 1 || pageSize < 1 || pageSize > 50)
        {
            _logger.LogDebug("Invalid pagination parameters Page: {page}, PageSize: {pageSize}", page, pageSize);
            return BadRequest("Invalid pagination parameters - MIN page = 1, MAX pageSize = 50 ");
        }

        var res = await _userService.GetAllAsync(page, pageSize);
        return res != null ? Ok(res) : NotFound("No users found");
    }

    // GET https://localhost:7070/api/v1/users/
    [HttpGet("{id}", Name = "GetUserById")]
    public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] Guid id)
    {
        _logger.LogInformation("Retrieving user with ID: {id}", id);

        var res = await _userService.GetByIdAsync(id);
        return res != null ? Ok(res) : NotFound($"No user with ID {id} was found"); // logge?????
    }

    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
        //var userId = User.FindFirst("UserId")?.Value; // brukes for å sjekke at en bruker kun kan endre sine gne checklists

    }

    // DELETE api/<UserController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }

    // POST https://localhost:7070/api/v1/users/register
    [HttpPost("register", Name = "RegisterUser")]
    public async Task<ActionResult<UserDTO>> RegisterUser([FromBody] UserRegistrationDTO dto)
    {
        _logger.LogInformation("Registering new user: {email}", dto.Email);

        var res = await _userService.RegisterUserAsync(dto);
        return res != null ? Ok(res) : BadRequest("Could not register new user");
    }
}
