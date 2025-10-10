using Knowball.Domain;
using Knowball.Domain.Repositories;


namespace Knowball.Infrastructure.Repositories
{
    public class CampeonatoRepository : ICampeonatoRepository
    {
        private readonly KnowballContext _context;

        public CampeonatoRepository(KnowballContext context)
        {
            _context = context;
        }

        public void Add(Campeonato entity)
        {
            _context.Campeonatos.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Campeonato> GetAll()
        {
            return _context.Campeonatos.ToList();
        }

        public Campeonato? GetById(int id)
        {
            return _context.Campeonatos.Find(id);
        }

        public void Remove(int id)
        {
            var entity = _context.Campeonatos.Find(id);
            if(entity != null)
            {
                _context.Campeonatos.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(Campeonato entity)
        {
            _context.Campeonatos.Update(entity);
            _context.SaveChanges();
        }
    }
}
