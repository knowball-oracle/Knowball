namespace Knowball.Domain.Repositories
{
    public interface IParticipacaoRepository
    {
        Participacao? GetByIds(int idPartida, int idEquipe);
        IEnumerable<Participacao> GetAll();
        void Add(Participacao participacao);
        void Update(Participacao participacao);
        void Remove(int idPartida, int idEquipe);
    }
}
