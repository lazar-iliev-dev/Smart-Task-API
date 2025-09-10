namespace Application.Interfaces;

public interface IHealthCheckService
{
        /// <summary>
        /// Checks the health of the application (DB, external services, etc.)
        /// Returns true if everything is OK, otherwise false.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default);
    
}