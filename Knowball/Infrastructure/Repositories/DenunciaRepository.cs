using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Infrastructure.Repositories
{
    public class DenunciaRepository : IDenunciaRepository
    {
        private readonly KnowballContext _context;

        public DenunciaRepository(KnowballContext context)
        {
            _context = context;
        }

        public void Add(Denuncia entity)
        {
            _context.Denuncias.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Denuncia> GetAll()
        {
            return _context.Denuncias.ToList();
        }

        public Denuncia GetById(int id)
        {
            return _context.Denuncias.Find(id);
        }

        public void Remove(int id)
        {
            var entity = _context.Denuncias.Find(id);
            if(entity != null)
            {
                _context.Denuncias.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(Denuncia entity)
        {
            _context.Denuncias.Update(entity);
            _context.SaveChanges();
        }
    }
}
