using Knowball.Models;

namespace Knowball.Domain
{
    public class Participacao
    {
        public int IdPartida { get; set; }
        public int IdEquipe { get; set; }
        public string Tipo { get; set; }

        public Partida Partida { get; set; }
        public Equipe Equipe { get; set; }
    }
}
