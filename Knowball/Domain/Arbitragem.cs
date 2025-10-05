using Knowball.Models;

namespace Knowball.Domain
{
    public class Arbitragem
    {
        public int IdPartida { get; set; }
        public int IdArbitro { get; set; }
        public string Funcao { get; set; }

        public bool FuncaoValida() =>
            Funcao == "Principal" || Funcao == "Assistente 1" || Funcao == "Assistente 2" || Funcao == "Quarto Árbitro";
    }
}
