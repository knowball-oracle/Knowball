using Knowball.Application.DTOs;

namespace Knowball.Application.Services
{
    public interface ICampeonatoService
    {
        CampeonatoDto CriarCampeonato(CampeonatoDto dto);
        IEnumerable<CampeonatoDto> ListarCampeonatos();
        CampeonatoDto ObterPorId(int id);
        void AtualizarCampeonato(int id, CampeonatoDto dto);
        void RemoverCampeonato(int id);
    }
}
