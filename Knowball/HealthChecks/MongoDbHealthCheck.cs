using Fiap.Knowball.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Fiap.Knowball.HealthChecks
{
    public class MongoDbHealthCheck : IHealthCheck
    {
        private readonly MongoDbSettings _settings;

        public MongoDbHealthCheck(IOptions<MongoDbSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var client = new MongoClient(_settings.ConnectionString);
                await client.ListDatabaseNamesAsync(cancellationToken);
                return HealthCheckResult.Healthy("Conexão com MongoDB bem-sucedida.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Falha na conexão com o MongoDB.", ex);
            }
        }   
    }
}
