using Knowball.Application.DTOs;
using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Application.Services
{
    public class ArbitroService : IArbitroService
    {
        private readonly IArbitroRepository _arbitroRepository;

        public ArbitroService(IArbitroRepository arbitroRepository)
        {
            _arbitroRepository = arbitroRepository;
        }

        public ArbitroDto CriarArbitro(ArbitroDto dto)
        {
            var arbitro = new Arbitro
            {
                Nome = dto.Nome,
                DataNascimento = dto.DataNascimento,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "Ativo" : dto.Status
            };

            if (!arbitro.StatusValido())
                throw new ArgumentException("Status inválido. Use: Ativo, Inativo ou Suspenso.");

            _arbitroRepository.Add(arbitro);

            dto.IdArbitro = arbitro.IdArbitro;
            dto.Status = arbitro.Status;
            return dto;
        }

        public IEnumerable<ArbitroDto> ListarArbitros()
        {
            return _arbitroRepository.GetAll()
                .Select(a => new ArbitroDto
                {
                    IdArbitro = a.IdArbitro,
                    Nome = a.Nome,
                    DataNascimento = a.DataNascimento,
                    Status = a.Status
                });
        }

        public ArbitroDto ObterPorId(int idArbitro)
        {
            var arbitro = _arbitroRepository.GetById(idArbitro);
            if (arbitro == null)
                return null;

            return new ArbitroDto
            {
                IdArbitro = arbitro.IdArbitro,
                Nome = arbitro.Nome,
                DataNascimento = arbitro.DataNascimento,
                Status = arbitro.Status
            };
        }

        public void AtualizarArbitro(int idArbitro, ArbitroDto dto)
        {
            var arbitro = _arbitroRepository.GetById(idArbitro);
            if (arbitro == null)
                throw new ArgumentException("Árbitro não encontrado.");

            arbitro.Nome = dto.Nome;
            arbitro.DataNascimento = dto.DataNascimento;
            arbitro.Status = string.IsNullOrWhiteSpace(dto.Status) ? "Ativo" : dto.Status;

            if (!arbitro.StatusValido())
                throw new ArgumentException("Status inválido. Use: Ativo, Inativo ou Suspenso.");

            _arbitroRepository.Update(arbitro);
        }

        public void RemoverArbitro(int idArbitro)
        {
            _arbitroRepository.Remove(idArbitro);
        }
    }
}
