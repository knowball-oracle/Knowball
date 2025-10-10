namespace Knowball.Domain.Repositories
{
    public interface IEquipeRepository
    {
        Equipe? GetById(int id);
        IEnumerable<Equipe> GetAll();
        void Add(Equipe equipe);
        void Update(Equipe equipe);
        void Remove(int id);
    }
}
