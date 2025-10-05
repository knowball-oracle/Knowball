using Knowball.Application.DTOs;

namespace Knowball.Application.Services
{
    public interface ICampeonatoService
    {
        void CriarCampeonato(CampeonatoDTO dto);
        IEnumerable<CampeonatoDTO> ListarCampeonatos();
        CampeonatoDTO ObterPorId(int id);
        void AtualizarCampeonato(int id, CampeonatoDTO dto);
        void RemoverCampeonato(int id);
    }
}
