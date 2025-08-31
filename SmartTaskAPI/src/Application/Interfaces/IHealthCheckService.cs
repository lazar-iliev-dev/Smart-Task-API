namespace Application.Interfaces;

public interface IHealthCheckService
{
        /// <summary>
        /// Prüft die Gesundheit der Anwendung (DB, externe Dienste, ...)
        /// Liefert true wenn alles ok ist, sonst false.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default);
    
}