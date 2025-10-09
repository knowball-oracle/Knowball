using Knowball.Domain.Repositories;
using Knowball.Models;

namespace Knowball.Infrastructure.Repositories
{
    public class PartidaRepository : IPartidaRepository
    {
        private readonly KnowballContext _context;

        public PartidaRepository(KnowballContext context)
        {
            _context = context;
        }

        public void Add(Partida entity)
        {
            _context.Partidas.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Partida> GetAll()
        {
            return _context.Partidas.ToList();
        }

        public Partida GetById(int id)
        {
            return _context.Partidas.Find(id);
        }

        public void Remove(int id)
        {
            var entity = _context.Partidas.Find(id);
            if(entity != null)
            {
                _context.Partidas.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(Partida entity)
        {
            _context.Partidas.Update(entity);
            _context.SaveChanges();
        }
    }
}
