using Knowball.Models;

namespace Knowball.Domain
{
    public class Arbitragem
    {
        public int IdPartida { get; set; }
        public int IdArbitro { get; set; }
        public string Funcao { get; set; }

        public Partida partida { get; set; }
        public Arbitro Arbitro { get; set; }
    }
}
