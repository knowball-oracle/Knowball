using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Domain;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Models;

namespace Fiap.Knowball.Application.Services
{
    public class PartidaService : IPartidaService
    {
        private readonly IPartidaRepository _repository;
        private readonly ILogger<PartidaService> _logger;

        public PartidaService(IPartidaRepository repository, ILogger<PartidaService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public PartidaDto CriarPartida(PartidaDto dto)
        {
            _logger.LogInformation("Criando partida: CampeonatoId={IdCampeonato}, Data={DataPartida}, Local={Local}",
                dto.IdCampeonato, dto.DataPartida, dto.Local);

            var partida = new Partida
            {
                IdCampeonato = dto.IdCampeonato,
                DataPartida = dto.DataPartida,
                Local = dto.Local,
                PlacarMandante = dto.PlacarMandante,
                PlacarVisitante = dto.PlacarVisitante,
                Participacoes = new List<Participacao>()
            };

            if (!partida.PlacarValido())
            {
                _logger.LogWarning("Placar inválido ao criar partida: Mandante={PlacarMandante}, Visitante={PlacarVisitante}",
                    dto.PlacarMandante, dto.PlacarVisitante);
                throw new BusinessException("Placar inválido");
            }

            if (!partida.DataValida())
            {
                _logger.LogWarning("Data inválida ao criar partida: {DataPartida}", dto.DataPartida);
                throw new BusinessException("Data da partida inválida");
            }

            _repository.Add(partida);
            _logger.LogInformation("Partida criada com sucesso: IdPartida={IdPartida}, CampeonatoId={IdCampeonato}",
                partida.IdPartida, partida.IdCampeonato);

            return new PartidaDto
            {
                IdPartida = partida.IdPartida,
                IdCampeonato = partida.IdCampeonato,
                DataPartida = partida.DataPartida,
                Local = partida.Local,
                PlacarMandante = partida.PlacarMandante,
                PlacarVisitante = partida.PlacarVisitante
            };
        }

        public IEnumerable<PartidaDto> ListarPartidas()
        {
            _logger.LogInformation("Listando todas as partidas");
            var partidas = _repository.GetAll();
            return partidas.Select(p => new PartidaDto
            {
                IdPartida = p.IdPartida,
                IdCampeonato = p.IdCampeonato,
                DataPartida = p.DataPartida,
                Local = p.Local,
                PlacarMandante = p.PlacarMandante,
                PlacarVisitante = p.PlacarVisitante
            });
        }

        public PartidaDto ObterPorId(int id)
        {
            _logger.LogInformation("Buscando partida: IdPartida={IdPartida}", id);

            var p = _repository.GetById(id);
            if (p == null)
            {
                _logger.LogWarning("Partida não encontrada: IdPartida={IdPartida}", id);
                return null;
            }

            return new PartidaDto
            {
                IdPartida = p.IdPartida,
                IdCampeonato = p.IdCampeonato,
                DataPartida = p.DataPartida,
                Local = p.Local,
                PlacarMandante = p.PlacarMandante,
                PlacarVisitante = p.PlacarVisitante
            };
        }

        public void AtualizarPartida(int id, PartidaDto dto)
        {
            _logger.LogInformation("Atualizando partida: IdPartida={IdPartida}", id);

            var p = _repository.GetById(id);
            if (p == null)
            {
                _logger.LogWarning("Partida não encontrada para atualização: IdPartida={IdPartida}", id);
                throw new BusinessException("Partida não encontrada");
            }

            p.IdCampeonato = dto.IdCampeonato;
            p.DataPartida = dto.DataPartida;
            p.Local = dto.Local;
            p.PlacarMandante = dto.PlacarMandante;
            p.PlacarVisitante = dto.PlacarVisitante;

            if (!p.PlacarValido())
            {
                _logger.LogWarning("Placar inválido ao atualizar IdPartida={IdPartida}: Mandante={PlacarMandante}, Visitante={PlacarVisitante}",
                    id, dto.PlacarMandante, dto.PlacarVisitante);
                throw new BusinessException("Placar inválido");
            }

            if (!p.DataValida())
            {
                _logger.LogWarning("Data inválida ao atualizar IdPartida={IdPartida}: {DataPartida}", id, dto.DataPartida);
                throw new BusinessException("Data da partida inválida");
            }

            _repository.Update(p);
            _logger.LogInformation("Partida atualizada com sucesso: IdPartida={IdPartida}", id);
        }

        public void RemoverPartida(int id)
        {
            _logger.LogInformation("Removendo partida: IdPartida={IdPartida}", id);
            _repository.Remove(id);
            _logger.LogInformation("Partida removida com sucesso: IdPartida={IdPartida}", id);
        }
    }
}