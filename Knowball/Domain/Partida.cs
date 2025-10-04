using Knowball.Domain;

namespace Knowball.Models
{
    public class Partida
    {
        public int IdPartida { get; set; }
        public int IdCampeonato { get; set; }
        public DateTime DataPartida { get; set; }
        public string Local { get; set; }
        public int PlacarMandante { get; set; }
        public int PlacarVisitante { get; set; }

        public Campeonato Campeonato { get; set; }

        public ICollection<Participacao> Participacoes { get; set; }

        public ICollection<Arbitragem> Arbitragens { get; set; }

        public ICollection<Denuncia> Denuncias { get; set; }

        public Partida()
        {
            Participacoes = new List<Participacao>();
            Arbitragens = new List<Arbitragem>();
            Denuncias = new List<Denuncia>();
        }
    }
}
