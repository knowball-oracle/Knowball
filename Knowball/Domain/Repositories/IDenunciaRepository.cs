namespace Knowball.Domain.Repositories
{
    public interface IDenunciaRepository
    {
        Denuncia GetById(int id);
        IEnumerable<Denuncia> GetAll();
        void Add (Denuncia denuncia);
        void Update(Denuncia denuncia);
        void Remove(int id);
    }
}
