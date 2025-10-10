namespace Knowball.Domain.Repositories
{
    public interface IArbitroRepository
    {
        Arbitro? GetById(int id);
        IEnumerable<Arbitro> GetAll();
        void Add(Arbitro arbitro);
        void Update(Arbitro arbitro);
        void Remove(int id);
    }
}
