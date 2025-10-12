using Knowball.Application.DTOs;
using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Application.Services
{
    public class EquipeService : IEquipeService
    {
        private readonly IEquipeRepository _equipeRepository;

        public EquipeService(IEquipeRepository equipeRepository)
        {
            _equipeRepository = equipeRepository;
        }

        public EquipeDto CriarEquipe(EquipeDto dto)
        {
            var equipe = new Equipe
            {
                Nome = dto.Nome,
                Cidade = dto.Cidade,
                Estado = dto.Estado
            };

            if (!equipe.DadosValidos())
                throw new ArgumentException("Dados da equipe inválidos.");

            _equipeRepository.Add(equipe);

            dto.IdEquipe = equipe.IdEquipe;
            return dto;
        }

        public IEnumerable<EquipeDto> ListarEquipes()
        {
            return _equipeRepository.GetAll()
                .Select(e => new EquipeDto
                {
                    IdEquipe = e.IdEquipe,
                    Nome = e.Nome,
                    Cidade = e.Cidade, // Ajuste se Categoria não for Cidade!
                    Estado = e.Estado
                });
        }

        public EquipeDto ObterPorId(int idEquipe)
        {
            var equipe = _equipeRepository.GetById(idEquipe);
            if (equipe == null)
                return null;

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
            var equipe = _equipeRepository.GetById(idEquipe);
            if (equipe == null)
                throw new ArgumentException("Equipe não encontrada.");

            equipe.Nome = dto.Nome;
            equipe.Cidade = dto.Cidade; 
            equipe.Estado = dto.Estado;

            if (!equipe.DadosValidos())
                throw new ArgumentException("Dados da equipe inválidos.");

            _equipeRepository.Update(equipe);
        }

        public void RemoverEquipe(int idEquipe)
        {
            _equipeRepository.Remove(idEquipe);
        }
    }
}
