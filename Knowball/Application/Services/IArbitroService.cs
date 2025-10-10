using Knowball.Application.DTOs;

namespace Knowball.Application.Services
{
    public interface IArbitroService
    {
        ArbitroDto CriarArbitro(ArbitroDto dto);
        IEnumerable<ArbitroDto> ListarArbitros();
        ArbitroDto ObterPorId(int idArbitro);
        void AtualizarArbitro(int idArbitro, ArbitroDto dto);
        void RemoverArbitro(int idArbitro);
    }
}
