using Knowball.Application.DTOs;
using Knowball.Application.Exceptions;
using Knowball.Domain;
using Knowball.Domain.Repositories;
using Knowball.Models;

namespace Knowball.Application.Services
{
    public class PartidaService : IPartidaService
    {
        private readonly IPartidaRepository _repository;

        public PartidaService(IPartidaRepository repository)
        {
            _repository = repository;
        }

        public PartidaDto CriarPartida(PartidaDto dto)
        {
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
                throw new BusinessException("Placar inválido");

            if (!partida.DataValida())
                throw new BusinessException("Data da partida inválida");

            _repository.Add(partida);

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
            var p = _repository.GetById(id);
            if (p == null) throw new BusinessException("Partida não encontrada");

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
            var p = _repository.GetById(id);
            if (p == null) throw new BusinessException("Partida não encontrada");

            p.IdCampeonato = dto.IdCampeonato;
            p.DataPartida = dto.DataPartida;
            p.Local = dto.Local;
            p.PlacarMandante = dto.PlacarMandante;
            p.PlacarVisitante = dto.PlacarVisitante;

            if (!p.PlacarValido())
                throw new BusinessException("Placar inválido");

            if (!p.DataValida())
                throw new BusinessException("Data da partida inválida");

            _repository.Update(p);
        }

        public void RemoverPartida(int id)
        {
            _repository.Remove(id);
        }
    }
}
