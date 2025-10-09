using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Infrastructure.Repositories
{
    public class EquipeRepository : IEquipeRepository
    {
        private readonly KnowballContext _context;

        public EquipeRepository(KnowballContext context)
        {
            _context = context;
        }

        public void Add(Equipe entity)
        {
            _context.Equipes.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Equipe> GetAll()
        {
            return _context.Equipes.ToList();
        }

        public Equipe GetById(int id)
        {
            return _context.Equipes.Find(id);
        }

        public void Remove(int id)
        {
            var entity = _context.Equipes.Find(id);
            if(entity != null)
            {
                _context.Equipes.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(Equipe entity)
        {
            _context.Equipes.Update(entity);
            _context.SaveChanges();
        }
    }
}
