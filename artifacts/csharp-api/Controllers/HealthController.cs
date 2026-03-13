using Microsoft.AspNetCore.Mvc;

namespace CsharpApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(HealthStatus), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new HealthStatus { Status = "ok" });
    }
}

public record HealthStatus
{
    public required string Status { get; init; }
}
