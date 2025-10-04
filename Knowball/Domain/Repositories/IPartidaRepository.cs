using Knowball.Models;

namespace Knowball.Domain.Repositories
{
    public interface IPartidaRepository
    {
        Partida GetById(int id);
        IEnumerable<Partida> GetAll();
        void Add(Partida partida);
        void Update(Partida partida);
        void Remove(int id);
    }
}
