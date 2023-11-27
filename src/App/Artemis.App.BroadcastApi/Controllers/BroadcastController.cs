using System.ComponentModel.DataAnnotations;
using Artemis.App.BroadcastApi.Data;
using Artemis.Data.Core;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.BroadcastApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class BroadcastController : ControllerBase
{
    private readonly BroadcastContext _context;

    private readonly ILogger<BroadcastController> _logger;

    public BroadcastController(
        ILogger<BroadcastController> logger,
        BroadcastContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public DataResult<string> SignIn([FromBody] SignInRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == request.UserName);

        return DataResult.Success("token", "³É¹¦");
    }
}

public class SignInRequest
{
    [Required] public required string UserName { get; set; }

    [Required] public required string Password { get; set; }
}