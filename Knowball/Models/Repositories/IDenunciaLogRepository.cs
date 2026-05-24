using Fiap.Knowball.Models;

namespace Fiap.Knowball.Models.Repositories;

public interface IDenunciaLogRepository
{
    Task RegistrarAsync(DenunciaLog log);
    Task<List<DenunciaLog>> ObterPorDenunciaAsync(int denunciaId);
}