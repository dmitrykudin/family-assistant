using Microsoft.AspNetCore.Mvc;

namespace FamilyAssistant.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
    private readonly ILogger<PingController> _logger;

    public PingController(ILogger<PingController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult Get()
    {
        _logger.LogInformation("Ping requested");

        return Ok();
    }
}
