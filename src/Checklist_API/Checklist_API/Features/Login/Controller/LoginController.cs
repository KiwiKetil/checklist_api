using Check_List_API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checklist_API.Features.Login.Controller;
[Route("api/v1/login")]
[ApiController]
public class LoginController : ControllerBase
{
    private IConfiguration _config; // trengs for tilgang til appsettings pga JWT secretkey etc ligger der
    private readonly CheckListDbContext _DbContext;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IConfiguration config, CheckListDbContext dbContext, ILogger<LoginController> logger)
    {
        _config = config;
        _DbContext = dbContext;
        _logger = logger;
    }
}
