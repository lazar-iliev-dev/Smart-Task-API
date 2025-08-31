using Application.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Services
{

        public class HealthCheckService : IHealthCheckService
        {
            private readonly AppDbContext _dbContext;


            public HealthCheckService(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }


            /// <summary>
            /// Sehr einfache Health-Prüfung: prüft, ob die DB erreichbar ist.
            /// Du kannst hier weitere Prüfungen ergänzen (Redis, externe APIs, ML-Service etc.).
            /// </summary>
            public async Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default)
            {
                try
                {
    // CanConnectAsync ist eine leichte, nicht-invasive Prüfung
                    return await _dbContext.Database.CanConnectAsync(cancellationToken);
                }
                catch
                {
                    return false;
                }
            }
        }

}    