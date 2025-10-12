using Knowball.Application.DTOs;
using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Application.Services
{
    public class ArbitragemService : IArbitragemService
    {
        private readonly IArbitragemRepository _arbitragemRepository;

        public ArbitragemService(IArbitragemRepository arbitragemRepository)
        {
            _arbitragemRepository = arbitragemRepository;
        }

        public ArbitragemDto CriarArbitragem(ArbitragemDto dto)
        {
            var arbitragem = new Arbitragem
            {
                IdPartida = dto.IdPartida,
                IdArbitro = dto.IdArbitro,
                Funcao = dto.Funcao
            };

            if (!arbitragem.FuncaoValida())
                throw new ArgumentException("Função inválida. Use: Principal, Assistente 1, Assistente 2 ou Quarto Árbitro.");

            _arbitragemRepository.Add(arbitragem);

            return dto;
        }

        public IEnumerable<ArbitragemDto> ListarArbitragens()
        {
            return _arbitragemRepository.GetAll()
                .Select(a => new ArbitragemDto
                {
                    IdPartida = a.IdPartida,
                    IdArbitro = a.IdArbitro,
                    Funcao = a.Funcao
                });
        }

        public ArbitragemDto ObterPorIds(int idPartida, int idArbitro)
        {
            var arbitragem = _arbitragemRepository.GetByIds(idPartida, idArbitro);
            if (arbitragem == null)
                return null;

            return new ArbitragemDto
            {
                IdPartida = arbitragem.IdPartida,
                IdArbitro = arbitragem.IdArbitro,
                Funcao = arbitragem.Funcao
            };
        }

        public void AtualizarArbitragem(int idPartida, int idArbitro, ArbitragemDto dto)
        {
            var arbitragem = _arbitragemRepository.GetByIds(idPartida, idArbitro);
            if (arbitragem == null)
                throw new ArgumentException("Arbitragem não encontrada.");

            arbitragem.Funcao = dto.Funcao;

            if (!arbitragem.FuncaoValida())
                throw new ArgumentException("Função inválida. Use: Principal, Assistente 1, Assistente 2 ou Quarto Árbitro.");

            _arbitragemRepository.Update(arbitragem);
        }

        public void RemoverArbitragem(int idPartida, int idArbitro)
        {
            _arbitragemRepository.Remove(idPartida, idArbitro);
        }
    }
}
