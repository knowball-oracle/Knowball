using Knowball.Application.DTOs;
using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Application.Services
{
    public class ParticipacaoService : IParticipacaoService
    {
        private readonly IParticipacaoRepository _participacaoRepository;

        public ParticipacaoService(IParticipacaoRepository participacaoRepository)
        {
            _participacaoRepository = participacaoRepository;
        }

        public ParticipacaoDto CriarParticipacao(ParticipacaoDto dto)
        {
            var participacao = new Participacao
            {
                IdPartida = dto.IdPartida,
                IdEquipe = dto.IdEquipe,
                Tipo = dto.Tipo
            };

            if (!participacao.TipoValido())
                throw new ArgumentException("Tipo de participação inválido. Use: Mandante ou Visitante.");

            _participacaoRepository.Add(participacao);

            return dto;
        }

        public IEnumerable<ParticipacaoDto> ListarParticipacoes()
        {
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
            var participacao = _participacaoRepository.GetByIds(idPartida, idEquipe);
            if (participacao == null)
                return null;

            return new ParticipacaoDto
            {
                IdPartida = participacao.IdPartida,
                IdEquipe = participacao.IdEquipe,
                Tipo = participacao.Tipo
            };
        }

        public void AtualizarParticipacao(int idPartida, int idEquipe, ParticipacaoDto dto)
        {
            var participacao = _participacaoRepository.GetByIds(idPartida, idEquipe);
            if (participacao == null)
                throw new ArgumentException("Participação não encontrada.");

            participacao.Tipo = dto.Tipo;

            if (!participacao.TipoValido())
                throw new ArgumentException("Tipo de participação inválido. Use: Mandante ou Visitante.");

            _participacaoRepository.Update(participacao);
        }

        public void RemoverParticipacao(int idPartida, int idEquipe)
        {
            _participacaoRepository.Remove(idPartida, idEquipe);
        }
    }
}
