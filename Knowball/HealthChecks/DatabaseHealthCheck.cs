using Fiap.Knowball.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fiap.Knowball.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly KnowballContext _context;
       
        public DatabaseHealthCheck(KnowballContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SELECT 1 FROM DUAL", cancellationToken);
                return HealthCheckResult.Healthy("Conexão com o banco de dados Oracle está OK.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    description: "Falha na conexão com o banco de dados Oracle.", 
                    exception: ex);
            }
        }
    }
}
