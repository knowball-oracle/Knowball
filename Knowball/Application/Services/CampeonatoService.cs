using System;
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

        public void CriarCampeonato(CampeonatoDTO dto)
        {
            var campeonato = new Campeonato
            {
                Nome = dto.Nome,
                Categoria = dto.Categoria,
                Ano = dto.Ano
            };

            if (!campeonato.AnoValido() || !campeonato.CategoriaValida())
                throw new BusinessException("Ano ou categoria inválidos");

            _repository.Add(campeonato);
        }

        public IEnumerable<CampeonatoDTO> ListarCampeonatos()
        {
            var campeonatos = _repository.GetAll();
            return campeonatos.Select(c => new CampeonatoDTO
            {
                Nome = c.Nome,
                Categoria = c.Categoria,
                Ano = c.Ano
            });
        }

        public CampeonatoDTO ObterPorId(int id)
        {
            var c = _repository.GetById(id);
            if (c == null) throw new BusinessException("Campeonato não encontrado");

            return new CampeonatoDTO
            {
                Nome = c.Nome,
                Categoria = c.Categoria,
                Ano = c.Ano
            };
        }

        public void AtualizarCampeonato(int id, CampeonatoDTO dto)
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
