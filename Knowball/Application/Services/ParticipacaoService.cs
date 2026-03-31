using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Domain;

namespace Fiap.Knowball.Application.Services
{
    public class ParticipacaoService : IParticipacaoService
    {
        private readonly IParticipacaoRepository _participacaoRepository;
        private readonly ILogger<ParticipacaoService> _logger;

        public ParticipacaoService(IParticipacaoRepository participacaoRepository, ILogger<ParticipacaoService> logger)
        {
            _participacaoRepository = participacaoRepository;
            _logger = logger;
        }

        public ParticipacaoDto CriarParticipacao(ParticipacaoDto dto)
        {
            _logger.LogInformation("Criando participação: PartidaId={IdPartida}, EquipeId={IdEquipe}, Tipo={Tipo}",
                dto.IdPartida, dto.IdEquipe, dto.Tipo);

            var participacao = new Participacao
            {
                IdPartida = dto.IdPartida,
                IdEquipe = dto.IdEquipe,
                Tipo = dto.Tipo
            };

            if (!participacao.TipoValido())
            {
                _logger.LogWarning("Tipo inválido ao criar participação: {Tipo}", dto.Tipo);
                throw new ArgumentException("Tipo de participação inválido. Use: Mandante ou Visitante.");
            }

            _participacaoRepository.Add(participacao);
            _logger.LogInformation("Participação criada com sucesso: PartidaId={IdPartida}, EquipeId={IdEquipe}",
                dto.IdPartida, dto.IdEquipe);

            return dto;
        }

        public IEnumerable<ParticipacaoDto> ListarParticipacoes()
        {
            _logger.LogInformation("Listando todas as participações");
            return _participacaoRepository.GetAll()
                .Select(p => new ParticipacaoDto
                {
                    IdPartida = p.IdPartida,
                    IdEquipe = p.IdEquipe,
                    Tipo = p.Tipo
                });
        }

        public ParticipacaoDto ObterPorIds(int idPartida, int idEquipe)
        {
            _logger.LogInformation("Buscando participação: PartidaId={IdPartida}, EquipeId={IdEquipe}",
                idPartida, idEquipe);

            var participacao = _participacaoRepository.GetByIds(idPartida, idEquipe);
            if (participacao == null)
            {
                _logger.LogWarning("Participação não encontrada: PartidaId={IdPartida}, EquipeId={IdEquipe}",
                    idPartida, idEquipe);
                return null;
            }

            return new ParticipacaoDto
            {
                IdPartida = participacao.IdPartida,
                IdEquipe = participacao.IdEquipe,
                Tipo = participacao.Tipo
            };
        }

        public void AtualizarParticipacao(int idPartida, int idEquipe, ParticipacaoDto dto)
        {
            _logger.LogInformation("Atualizando participação: PartidaId={IdPartida}, EquipeId={IdEquipe}",
                idPartida, idEquipe);

            var participacao = _participacaoRepository.GetByIds(idPartida, idEquipe);
            if (participacao == null)
            {
                _logger.LogWarning("Participação não encontrada para atualização: PartidaId={IdPartida}, EquipeId={IdEquipe}",
                    idPartida, idEquipe);
                throw new ArgumentException("Participação não encontrada.");
            }

            participacao.Tipo = dto.Tipo;

            if (!participacao.TipoValido())
            {
                _logger.LogWarning("Tipo inválido ao atualizar participação PartidaId={IdPartida}, EquipeId={IdEquipe}: {Tipo}",
                    idPartida, idEquipe, dto.Tipo);
                throw new ArgumentException("Tipo de participação inválido. Use: Mandante ou Visitante.");
            }

            _participacaoRepository.Update(participacao);
            _logger.LogInformation("Participação atualizada com sucesso: PartidaId={IdPartida}, EquipeId={IdEquipe}",
                idPartida, idEquipe);
        }

        public void RemoverParticipacao(int idPartida, int idEquipe)
        {
            _logger.LogInformation("Removendo participação: PartidaId={IdPartida}, EquipeId={IdEquipe}",
                idPartida, idEquipe);
            _participacaoRepository.Remove(idPartida, idEquipe);
            _logger.LogInformation("Participação removida com sucesso: PartidaId={IdPartida}, EquipeId={IdEquipe}",
                idPartida, idEquipe);
        }
    }
}