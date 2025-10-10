using Knowball.Application.DTOs;

namespace Knowball.Application.Services
{
    public interface IDenunciaService
    {
        DenunciaDto CriarDenuncia(DenunciaDto dto);
        IEnumerable<DenunciaDto> ListarDenuncias();
        DenunciaDto ObterPorId(int id);
        void AtualizarDenuncia(int id, DenunciaDto dto);
        void RemoverDenuncia(int id);
    }
}
