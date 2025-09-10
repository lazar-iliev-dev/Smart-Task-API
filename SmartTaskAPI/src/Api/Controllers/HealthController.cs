using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Health;


namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
    public class HealthController : ControllerBase{
    private readonly Application.Interfaces.IHealthCheckService _service;

    public HealthController(Application.Interfaces.IHealthCheckService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await _service.IsHealthyAsync(ct);
        return Ok(result);
    }
}
