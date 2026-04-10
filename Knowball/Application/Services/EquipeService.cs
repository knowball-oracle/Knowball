using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Models;

namespace Fiap.Knowball.Application.Services
{
    public class EquipeService : IEquipeService
    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly ILogger<EquipeService> _logger;

        public EquipeService(IEquipeRepository equipeRepository, ILogger<EquipeService> logger)
        {
            _equipeRepository = equipeRepository;
            _logger = logger;
        }

        public EquipeDto CriarEquipe(EquipeDto dto)
        {
            _logger.LogInformation("Criando equipe: Nome={Nome}, Cidade={Cidade}, Estado={Estado}",
                dto.Nome, dto.Cidade, dto.Estado);

            var equipe = new Equipe
            {
                Nome = dto.Nome,
                Cidade = dto.Cidade,
                Estado = dto.Estado
            };

            if (!equipe.DadosValidos())
            {
                _logger.LogWarning("Dados inválidos ao criar equipe: Nome={Nome}, Cidade={Cidade}, Estado={Estado}",
                    dto.Nome, dto.Cidade, dto.Estado);
                throw new ArgumentException("Dados da equipe inválidos.");
            }

            _equipeRepository.Add(equipe);
            _logger.LogInformation("Equipe criada com sucesso: IdEquipe={IdEquipe}, Nome={Nome}",
                equipe.IdEquipe, equipe.Nome);

            dto.IdEquipe = equipe.IdEquipe;
            return dto;
        }

        public IEnumerable<EquipeDto> ListarEquipes()
        {
            _logger.LogInformation("Listando todas as equipes");
            return _equipeRepository.GetAll()
                .Select(e => new EquipeDto
                {
                    IdEquipe = e.IdEquipe,
                    Nome = e.Nome,
                    Cidade = e.Cidade,
                    Estado = e.Estado
                });
        }

        public EquipeDto ObterPorId(int idEquipe)
        {
            _logger.LogInformation("Buscando equipe: IdEquipe={IdEquipe}", idEquipe);

            var equipe = _equipeRepository.GetById(idEquipe);
            if (equipe == null)
            {
                _logger.LogWarning("Equipe não encontrada: IdEquipe={IdEquipe}", idEquipe);
                return null;
            }

            return new EquipeDto
            {
                IdEquipe = equipe.IdEquipe,
                Nome = equipe.Nome,
                Cidade = equipe.Cidade,
                Estado = equipe.Estado
            };
        }

        public void AtualizarEquipe(int idEquipe, EquipeDto dto)
        {
            _logger.LogInformation("Atualizando equipe: IdEquipe={IdEquipe}", idEquipe);

            var equipe = _equipeRepository.GetById(idEquipe);
            if (equipe == null)
            {
                _logger.LogWarning("Equipe não encontrada para atualização: IdEquipe={IdEquipe}", idEquipe);
                throw new ArgumentException("Equipe não encontrada.");
            }

            equipe.Nome = dto.Nome;
            equipe.Cidade = dto.Cidade;
            equipe.Estado = dto.Estado;

            if (!equipe.DadosValidos())
            {
                _logger.LogWarning("Dados inválidos ao atualizar equipe IdEquipe={IdEquipe}: Nome={Nome}, Estado={Estado}",
                    idEquipe, dto.Nome, dto.Estado);
                throw new ArgumentException("Dados da equipe inválidos.");
            }

            _equipeRepository.Update(equipe);
            _logger.LogInformation("Equipe atualizada com sucesso: IdEquipe={IdEquipe}", idEquipe);
        }

        public void RemoverEquipe(int idEquipe)
        {
            _logger.LogInformation("Removendo equipe: IdEquipe={IdEquipe}", idEquipe);
            _equipeRepository.Remove(idEquipe);
            _logger.LogInformation("Equipe removida com sucesso: IdEquipe={IdEquipe}", idEquipe);
        }
    }
}