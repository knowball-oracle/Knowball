using Knowball.Application.DTOs;

namespace Knowball.Application.Services
{
    public interface IEquipeService
    {
        EquipeDto CriarEquipe(EquipeDto dto);
        IEnumerable<EquipeDto> ListarEquipes();
        EquipeDto ObterPorId(int idEquipe);
        void AtualizarEquipe(int idEquipe, EquipeDto equipe);
        void RemoverEquipe(int idEquipe);
    }
}
