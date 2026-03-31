using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Domain;

namespace Fiap.Knowball.Application.Services
{
    public class ArbitragemService : IArbitragemService
    {
        private readonly IArbitragemRepository _arbitragemRepository;
        private readonly ILogger<ArbitragemService> _logger;

        public ArbitragemService(IArbitragemRepository arbitragemRepository, ILogger<ArbitragemService> logger)
        {
            _arbitragemRepository = arbitragemRepository;
            _logger = logger;
        }

        public ArbitragemDto CriarArbitragem(ArbitragemDto dto)
        {
            _logger.LogInformation("Criando arbitragem para PartidaId={IdPartida}, ArbitroId={IdArbitro}, Funcao={Funcao}",
                dto.IdPartida, dto.IdArbitro, dto.Funcao);

            var arbitragem = new Arbitragem
            {
                IdPartida = dto.IdPartida,
                IdArbitro = dto.IdArbitro,
                Funcao = dto.Funcao
            };

            if (!arbitragem.FuncaoValida())
            {
                _logger.LogWarning("Função inválida ao criar arbitragem: {Funcao}", dto.Funcao);
                throw new ArgumentException("Função inválida. Use: Principal, Assistente 1, Assistente 2 ou Quarto Árbitro.");
            }

            _arbitragemRepository.Add(arbitragem);
            _logger.LogInformation("Arbitragem criada com sucesso para PartidaId={IdPartida}, ArbitroId={IdArbitro}",
                dto.IdPartida, dto.IdArbitro);

            return dto;
        }

        public IEnumerable<ArbitragemDto> ListarArbitragens()
        {
            _logger.LogInformation("Listando todas as arbitragens");
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
            _logger.LogInformation("Buscando arbitragem para PartidaId={IdPartida}, ArbitroId={IdArbitro}",
                idPartida, idArbitro);

            var arbitragem = _arbitragemRepository.GetByIds(idPartida, idArbitro);
            if (arbitragem == null)
            {
                _logger.LogWarning("Arbitragem não encontrada para PartidaId={IdPartida}, ArbitroId={IdArbitro}",
                    idPartida, idArbitro);
                return null;
            }

            return new ArbitragemDto
            {
                IdPartida = arbitragem.IdPartida,
                IdArbitro = arbitragem.IdArbitro,
                Funcao = arbitragem.Funcao
            };
        }

        public void AtualizarArbitragem(int idPartida, int idArbitro, ArbitragemDto dto)
        {
            _logger.LogInformation("Atualizando arbitragem PartidaId={IdPartida}, ArbitroId={IdArbitro}",
                idPartida, idArbitro);

            var arbitragem = _arbitragemRepository.GetByIds(idPartida, idArbitro);
            if (arbitragem == null)
            {
                _logger.LogWarning("Arbitragem não encontrada para atualização: PartidaId={IdPartida}, ArbitroId={IdArbitro}",
                    idPartida, idArbitro);
                throw new ArgumentException("Arbitragem não encontrada.");
            }

            arbitragem.Funcao = dto.Funcao;

            if (!arbitragem.FuncaoValida())
            {
                _logger.LogWarning("Função inválida ao atualizar arbitragem: {Funcao}", dto.Funcao);
                throw new ArgumentException("Função inválida. Use: Principal, Assistente 1, Assistente 2 ou Quarto Árbitro.");
            }

            _arbitragemRepository.Update(arbitragem);
            _logger.LogInformation("Arbitragem atualizada com sucesso: PartidaId={IdPartida}, ArbitroId={IdArbitro}",
                idPartida, idArbitro);
        }

        public void RemoverArbitragem(int idPartida, int idArbitro)
        {
            _logger.LogInformation("Removendo arbitragem PartidaId={IdPartida}, ArbitroId={IdArbitro}",
                idPartida, idArbitro);
            _arbitragemRepository.Remove(idPartida, idArbitro);
            _logger.LogInformation("Arbitragem removida com sucesso: PartidaId={IdPartida}, ArbitroId={IdArbitro}",
                idPartida, idArbitro);
        }
    }
}