using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Infrastructure.Repositories
{
    public class ArbitroRepository : IArbitroRepository
    {
        private readonly KnowballContext _context;

        public ArbitroRepository(KnowballContext context)
        {
            _context = context;
        }

        public void Add(Arbitro entity)
        {
            _context.Arbitros.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Arbitro> GetAll()
        {
            return _context.Arbitros.ToList();
        }

        public Arbitro? GetById(int id)
        {
            return _context.Arbitros.Find(id);
        }

        public void Remove(int id)
        {
            var entity = _context.Arbitros.Find(id);
            if (entity != null)
            {
                _context.Arbitros.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(Arbitro entity)
        {
            _context.Arbitros.Update(entity);
            _context.SaveChanges();
        }
    }
}
