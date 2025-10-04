using Knowball.Models;

namespace Knowball.Domain
{
    public class Campeonato
    {
        public int IdCampeonato { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public int Ano { get; set; }
        public ICollection<Partida> Partidas { get; set; }

        public Campeonato()
        {
            Partidas = new List<Partida>();
        }
    }
}
