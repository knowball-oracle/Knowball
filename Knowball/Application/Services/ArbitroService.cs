using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Domain;

namespace Fiap.Knowball.Application.Services
{
    public class ArbitroService : IArbitroService
    {
        private readonly IArbitroRepository _arbitroRepository;
        private readonly ILogger<ArbitroService> _logger;

        public ArbitroService(IArbitroRepository arbitroRepository, ILogger<ArbitroService> logger)
        {
            _arbitroRepository = arbitroRepository;
            _logger = logger;
        }

        public ArbitroDto CriarArbitro(ArbitroDto dto)
        {
            _logger.LogInformation("Criando árbitro: Nome={Nome}", dto.Nome);

            var arbitro = new Arbitro
            {
                Nome = dto.Nome,
                DataNascimento = dto.DataNascimento,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "Ativo" : dto.Status
            };

            if (!arbitro.StatusValido())
            {
                _logger.LogWarning("Status inválido ao criar árbitro: {Status}", arbitro.Status);
                throw new ArgumentException("Status inválido. Use: Ativo, Inativo ou Suspenso.");
            }

            _arbitroRepository.Add(arbitro);
            _logger.LogInformation("Árbitro criado com sucesso: IdArbitro={IdArbitro}, Nome={Nome}",
                arbitro.IdArbitro, arbitro.Nome);

            dto.IdArbitro = arbitro.IdArbitro;
            dto.Status = arbitro.Status;
            return dto;
        }

        public IEnumerable<ArbitroDto> ListarArbitros()
        {
            _logger.LogInformation("Listando todos os árbitros");
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
            _logger.LogInformation("Buscando árbitro: IdArbitro={IdArbitro}", idArbitro);

            var arbitro = _arbitroRepository.GetById(idArbitro);
            if (arbitro == null)
            {
                _logger.LogWarning("Árbitro não encontrado: IdArbitro={IdArbitro}", idArbitro);
                return null;
            }

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
            _logger.LogInformation("Atualizando árbitro: IdArbitro={IdArbitro}", idArbitro);

            var arbitro = _arbitroRepository.GetById(idArbitro);
            if (arbitro == null)
            {
                _logger.LogWarning("Árbitro não encontrado para atualização: IdArbitro={IdArbitro}", idArbitro);
                throw new ArgumentException("Árbitro não encontrado.");
            }

            arbitro.Nome = dto.Nome;
            arbitro.DataNascimento = dto.DataNascimento;
            arbitro.Status = string.IsNullOrWhiteSpace(dto.Status) ? "Ativo" : dto.Status;

            if (!arbitro.StatusValido())
            {
                _logger.LogWarning("Status inválido ao atualizar árbitro IdArbitro={IdArbitro}: {Status}",
                    idArbitro, arbitro.Status);
                throw new ArgumentException("Status inválido. Use: Ativo, Inativo ou Suspenso.");
            }

            _arbitroRepository.Update(arbitro);
            _logger.LogInformation("Árbitro atualizado com sucesso: IdArbitro={IdArbitro}", idArbitro);
        }

        public void RemoverArbitro(int idArbitro)
        {
            _logger.LogInformation("Removendo árbitro: IdArbitro={IdArbitro}", idArbitro);
            _arbitroRepository.Remove(idArbitro);
            _logger.LogInformation("Árbitro removido com sucesso: IdArbitro={IdArbitro}", idArbitro);
        }
    }
}