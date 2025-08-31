using Application.Interfaces;

namespace Application.UseCases.Health;

public class CheckHealthHandler
{
    private readonly IHealthCheckService _healthService;


    public CheckHealthHandler(IHealthCheckService healthService)
    {
        _healthService = healthService;
    }


    public Task<bool> HandleAsync(CancellationToken cancellationToken = default)
    {
        return _healthService.IsHealthyAsync(cancellationToken);
    }
}