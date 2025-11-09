using Knowball.Application.DTOs;

namespace Knowball.Application.Services
{
    public interface IPartidaService
    {
        PartidaDto CriarPartida(PartidaDto dto);
        IEnumerable<PartidaDto> ListarPartidas();
        PartidaDto ObterPorId(int idPartida);
        void AtualizarPartida(int idPartida, PartidaDto dto);
        void RemoverPartida(int idPartida);
    }
}