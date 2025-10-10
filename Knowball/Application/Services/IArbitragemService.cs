using Knowball.Application.DTOs;

namespace Knowball.Application.Services
{
    public interface IArbitragemService
    {
        ArbitragemDto CriarArbitragem(ArbitragemDto dto);
        IEnumerable<ArbitragemDto> ListarArbitragens();
        ArbitragemDto ObterPorIds(int idPartida, int idArbitro);
        void AtualizarArbitragem(int idPartida, int idArbitro, ArbitragemDto dto);
        void RemoverArbitragem(int idPartida, int idArbitro);
    }
}
