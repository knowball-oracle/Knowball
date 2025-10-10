namespace Knowball.Domain.Repositories
{
    public interface IArbitragemRepository
    {
        Arbitragem? GetByIds(int idPartida, int idArbitro);
        IEnumerable<Arbitragem> GetAll();
        void Add(Arbitragem arbitragem);
        void Update(Arbitragem arbitragem);
        void Remove(int idPartida, int idArbitro);
    }
}
