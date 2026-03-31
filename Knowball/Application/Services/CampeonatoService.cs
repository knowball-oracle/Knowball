using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Domain;

namespace Fiap.Knowball.Application.Services
{
    public class CampeonatoService : ICampeonatoService
    {
        private readonly ICampeonatoRepository _repository;
        private readonly ILogger<CampeonatoService> _logger;

        public CampeonatoService(ICampeonatoRepository repository, ILogger<CampeonatoService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
        }

        public CampeonatoDto CriarCampeonato(CampeonatoDto dto)
        {
            _logger.LogInformation("Criando campeonato: Nome={Nome}, Categoria={Categoria}, Ano={Ano}",
                dto.Nome, dto.Categoria, dto.Ano);

            var campeonato = new Campeonato
            {
                Nome = dto.Nome,
                Categoria = dto.Categoria,
                Ano = dto.Ano
            };

            if (!campeonato.AnoValido() || !campeonato.CategoriaValida())
            {
                _logger.LogWarning("Dados inválidos ao criar campeonato: Ano={Ano}, Categoria={Categoria}",
                    dto.Ano, dto.Categoria);
                throw new BusinessException("Ano ou categoria inválidos");
            }

            _repository.Add(campeonato);
            _logger.LogInformation("Campeonato criado com sucesso: IdCampeonato={IdCampeonato}, Nome={Nome}",
                campeonato.IdCampeonato, campeonato.Nome);

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
            _logger.LogInformation("Listando todos os campeonatos");
            var campeonatos = _repository.GetAll();
            return campeonatos.Select(c => new CampeonatoDto
            {
                IdCampeonato = c.IdCampeonato,
                Nome = c.Nome,
                Categoria = c.Categoria,
                Ano = c.Ano
            });
        }

        public CampeonatoDto ObterPorId(int id)
        {
            _logger.LogInformation("Buscando campeonato: IdCampeonato={IdCampeonato}", id);

            var c = _repository.GetById(id);
            if (c == null)
            {
                _logger.LogWarning("Campeonato não encontrado: IdCampeonato={IdCampeonato}", id);
                return null;
            }

            return new CampeonatoDto
            {
                IdCampeonato = c.IdCampeonato,
                Nome = c.Nome,
                Ano = c.Ano,
                Categoria = c.Categoria
            };
        }

        public void AtualizarCampeonato(int id, CampeonatoDto dto)
        {
            _logger.LogInformation("Atualizando campeonato: IdCampeonato={IdCampeonato}", id);

            var c = _repository.GetById(id);
            if (c == null)
            {
                _logger.LogWarning("Campeonato não encontrado para atualização: IdCampeonato={IdCampeonato}", id);
                throw new BusinessException("Campeonato não encontrado");
            }

            c.Nome = dto.Nome;
            c.Categoria = dto.Categoria;
            c.Ano = dto.Ano;

            if (!c.AnoValido() || !c.CategoriaValida())
            {
                _logger.LogWarning("Dados inválidos ao atualizar campeonato IdCampeonato={IdCampeonato}: Ano={Ano}, Categoria={Categoria}",
                    id, dto.Ano, dto.Categoria);
                throw new BusinessException("Ano ou categoria inválidos");
            }

            _repository.Update(c);
            _logger.LogInformation("Campeonato atualizado com sucesso: IdCampeonato={IdCampeonato}", id);
        }

        public void RemoverCampeonato(int id)
        {
            _logger.LogInformation("Removendo campeonato: IdCampeonato={IdCampeonato}", id);

            var c = _repository.GetById(id);
            if (c == null)
            {
                _logger.LogWarning("Campeonato não encontrado para remoção: IdCampeonato={IdCampeonato}", id);
                throw new BusinessException("Campeonato não encontrado");
            }

            _repository.Remove(id);
            _logger.LogInformation("Campeonato removido com sucesso: IdCampeonato={IdCampeonato}", id);
        }
    }
}