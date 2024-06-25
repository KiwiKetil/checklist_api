using Check_List_API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checklist_API.Features.Login.Controller;
[Route("api/v1/login")]
[ApiController]
public class LoginController : ControllerBase
{
    private IConfiguration _config;
    private readonly CheckListDbContext _DbContext;
    private readonly ILogger<LoginController> _logger;
    public LoginController()
    {
        
    }
}
