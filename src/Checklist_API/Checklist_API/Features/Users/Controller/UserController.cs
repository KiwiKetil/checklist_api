using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Checklist_API.Features.Users.Controller;
[Route("api/v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    //GET: api/<UserController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll() // husk paginering og asNoTracking() i dbcontext
    {
        _logger.LogInformation("Getting all Users");
        var res = await _userService.GetAllAsync();

        return res != null ? Ok(res) : NotFound("Could not find any users");
    }

    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UserController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }

    // POST api/<UserController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }
}
