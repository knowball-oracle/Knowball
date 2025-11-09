using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Infrastructure.Repositories
{
    public class ParticipacaoRepository : IParticipacaoRepository
    {

        private readonly KnowballContext _context;

        public ParticipacaoRepository(KnowballContext context)
        {
            _context = context;
        }

        public void Add(Participacao entity)
        {
            _context.Participacoes.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Participacao> GetAll()
        {
            return _context.Participacoes.ToList();
        }

        public Participacao? GetByIds(int idPartida, int idEquipe)
        {
            return _context.Participacoes
                .FirstOrDefault(p => p.IdPartida == idPartida && p.IdEquipe == idEquipe);
        }

        public void Remove(int idPartida, int idEquipe)
        {
            var entity = GetByIds(idPartida, idEquipe);
            if (entity != null)
            {
                _context.Participacoes.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(Participacao entity)
        {
            _context.Participacoes.Update(entity);
            _context.SaveChanges();
        }
    }
}