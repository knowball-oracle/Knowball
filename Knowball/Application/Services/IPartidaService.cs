using Knowball.Application.DTOs;

namespace Knowball.Application.Services
{
    public interface IPartidaService
    {
        void CriarPartida(PartidaDTO dto);
        IEnumerable<PartidaDTO> ListarPartidas();
        PartidaDTO ObterPorId(int id);
        void AtualizarPartida(int id, PartidaDTO dto);
        void RemoverPartida(int id);
    }
}
