namespace Knowball.Domain.Repositories
{
    public interface ICampeonatoRepository
    {
        Campeonato? GetById(int id);
        IEnumerable<Campeonato> GetAll();
        void Add(Campeonato campeonato);
        void Update(Campeonato campeonato);
        void Remove(int id);
    }
}
