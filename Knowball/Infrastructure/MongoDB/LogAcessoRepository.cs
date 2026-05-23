using Fiap.Knowball.Configuration;
using Fiap.Knowball.Models.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Fiap.Knowball.Infrastructure.MongoDB
{
    public class LogAcessoRepository : ILogAcessoRepository
    {
        private readonly IMongoCollection<LogAcesso> _collection;

        public LogAcessoRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<LogAcesso>(settings.Value.LogAcessoCollection);
        }

        public async Task RegistrarAsync(LogAcesso log)
            => await _collection.InsertOneAsync(log);

        public async Task<List<LogAcesso>> ObterPorUsuarioAsync(string usuario, int limite = 50)
            => await _collection.Find(l => l.Usuario == usuario)
                .SortByDescending(l => l.DataHora)
                .Limit(limite)
                .ToListAsync();

        public async Task<List<LogAcesso>> ObterRecentesAsync(int limite = 100)
            => await _collection.Find(_ => true)
                .SortByDescending(l => l.DataHora)
                .Limit(limite)
                .ToListAsync();
    }
}
