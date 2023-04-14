using Microsoft.AspNetCore.Mvc;

namespace Artemis.Test.WebApiSnippets.Controllers;

[ApiController]
[Route("[controller]")]
public class SnippetController : ControllerBase
{
    private readonly ILogger<SnippetController> _logger;

    public SnippetController(ILogger<SnippetController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetResponse")]
    public ResponseEntity Get()
    {
        return new ResponseEntity();
    }
}