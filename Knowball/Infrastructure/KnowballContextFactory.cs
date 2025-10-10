using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Knowball.Infrastructure
{
    public class KnowballContextFactory : IDesignTimeDbContextFactory<KnowballContext>
    {
        public KnowballContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            connectionString = connectionString.Replace("${DB_USER}", dbUser ?? "");
            connectionString = connectionString.Replace("${DB_PASSWORD}", dbPassword ?? "");

            var optionsBuilder = new DbContextOptionsBuilder<KnowballContext>();
            optionsBuilder.UseOracle(connectionString);

            return new KnowballContext(optionsBuilder.Options);
        }
    }
}
