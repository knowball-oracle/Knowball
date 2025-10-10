using Knowball.Application.DTOs;

namespace Knowball.Application.Services
{
    public interface IParticipacaoService
    {
        ParticipacaoDto CriarParticipacao(ParticipacaoDto dto);
        IEnumerable<ParticipacaoDto> ListarParticipacoes();
        ParticipacaoDto ObterPorIds(int idPartida, int idEquipe);
        void AtualizarParticipacao(int idPartida, int idEquipe, ParticipacaoDto dto);
        void RemoverParticipacao(int idPartida, int idEquipe);
    }
}
