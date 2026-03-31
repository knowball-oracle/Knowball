using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fiap.Knowball.HealthChecks
{
    public class ApiHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var isHealthy = true;

            return Task.FromResult(isHealthy
                ? HealthCheckResult.Healthy("API está saudável.")
                : HealthCheckResult.Unhealthy("API com falha."));
        }
    }
}
