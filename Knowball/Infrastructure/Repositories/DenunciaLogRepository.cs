using Fiap.Knowball.Infrastructure;
using Fiap.Knowball.Models;
using Fiap.Knowball.Models.Repositories;
using MongoDB.Driver;

namespace Fiap.Knowball.Infrastructure.Repositories;

public class DenunciaLogRepository : IDenunciaLogRepository
{
    private readonly MongoDbContext _context;

    public DenunciaLogRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarAsync(DenunciaLog log) =>
        await _context.DenunciaLogs.InsertOneAsync(log);

    public async Task<List<DenunciaLog>> ObterPorDenunciaAsync(int denunciaId) =>
        await _context.DenunciaLogs
            .Find(x => x.DenunciaId == denunciaId)
            .SortByDescending(x => x.Timestamp)
            .ToListAsync();
}