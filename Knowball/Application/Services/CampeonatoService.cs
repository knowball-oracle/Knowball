
using Knowball.Application.DTOs;
using Knowball.Application.Exceptions;
using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Application.Services
{
    public class CampeonatoService : ICampeonatoService
    {
        private readonly ICampeonatoRepository _repository;

        public CampeonatoService(ICampeonatoRepository repository)
        {
            _repository = repository;
        }

        public CampeonatoDto CriarCampeonato(CampeonatoDto dto)
        {
            var campeonato = new Campeonato
            {
                IdCampeonato = dto.IdCampeonato,
                Nome = dto.Nome,
                Categoria = dto.Categoria,
                Ano = dto.Ano
            };

            if (!campeonato.AnoValido() || !campeonato.CategoriaValida())
                throw new BusinessException("Ano ou categoria inválidos");

            _repository.Add(campeonato);

            return new CampeonatoDto
            {
                IdCampeonato = campeonato.IdCampeonato,
                Nome = campeonato.Nome,
                Categoria = campeonato.Categoria,
                Ano = campeonato.Ano
            };
        }

        public IEnumerable<CampeonatoDto> ListarCampeonatos()
        {
            var campeonatos = _repository.GetAll();
            return campeonatos.Select(c => new CampeonatoDto
            {
                Nome = c.Nome,
                Categoria = c.Categoria,
                Ano = c.Ano
            });
        }

        public CampeonatoDto ObterPorId(int id)
        {
            var c = _repository.GetById(id);
            if (c == null) throw new BusinessException("Campeonato não encontrado");

            return new CampeonatoDto
            {
                IdCampeonato = c.IdCampeonato,
                Nome = c.Nome,
                Categoria = c.Categoria,
                Ano = c.Ano
            };
        }

        public void AtualizarCampeonato(int id, CampeonatoDto dto)
        {
            var c = _repository.GetById(id);
            if (c == null) throw new BusinessException("Campeonato não encontrado");

            c.Nome = dto.Nome;
            c.Categoria = dto.Categoria;
            c.Ano = dto.Ano;

            if (!c.AnoValido() || !c.CategoriaValida())
                throw new BusinessException("Ano ou categoria inválidos");

            _repository.Update(c);
        }

        public void RemoverCampeonato(int id)
        {
            _repository.Remove(id);
        }
    }
}
