using MongoDB.Driver;
using Fiap.Knowball.Models;

namespace Fiap.Knowball.Infrastructure;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        _database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
    }

    public IMongoCollection<DenunciaLog> DenunciaLogs =>
        _database.GetCollection<DenunciaLog>("denuncia_logs");
}