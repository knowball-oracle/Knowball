using Fiap.Knowball.Models.MongoDB;

namespace Fiap.Knowball.Infrastructure.MongoDB
{
    public interface ILogAcessoRepository
    {
        Task RegistrarAsync(LogAcesso log);
        Task<List<LogAcesso>> ObterPorUsuarioAsync(string usuario, int limite = 50);
        Task<List<LogAcesso>> ObterRecentesAsync(int limite = 100);
    }
}
