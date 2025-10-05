using Knowball.Application.DTOs;

namespace Knowball.Application.Services
{
    public interface IDenunciaService
    {
        void CriarDenuncia(DenunciaDTO dto);
        IEnumerable<DenunciaDTO> ListarDenuncias();
        DenunciaDTO ObterPorId(int id);
        void AtualizarDenuncia(int id, DenunciaDTO dto);
        void RemoverDenuncia(int id);
    }
}
