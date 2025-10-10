using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Infrastructure.Repositories
{
    public class ArbitragemRepository : IArbitragemRepository
    {
        private readonly KnowballContext _context;

        public ArbitragemRepository(KnowballContext context)
        {
            _context = context;
        }

        public void Add(Arbitragem entity)
        {
            _context.Arbitragens.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Arbitragem> GetAll()
        {
            return _context.Arbitragens.ToList();
        }

        public Arbitragem? GetByIds(int idPartida, int idArbitro)
        {
            return _context.Arbitragens
                .FirstOrDefault(a => a.IdPartida == idPartida && a.IdArbitro == idArbitro);
        }

        public void Remove(int idPartida, int idArbitro)
        {
            var entity = GetByIds(idPartida, idArbitro);
            if(entity != null)
            {
                _context.Arbitragens.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(Arbitragem entity)
        {
            _context.Arbitragens.Update(entity);
            _context.SaveChanges();
        }
    }
}
