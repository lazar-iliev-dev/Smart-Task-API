using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Health;


namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly CheckHealthHandler _handler;


    public HealthController(CheckHealthHandler handler)
    {
        _handler = handler;
    }


    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var healthy = await _handler.HandleAsync(cancellationToken);
        return Ok(new { healthy });
    }
}