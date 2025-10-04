using Knowball.Models;

namespace Knowball.Domain
{
    public class Arbitragem
    {
        public int IdPartida { get; set; }
        public int IdArbitro { get; set; }
        public string Funcao { get; set; }

        public bool FuncaoValida()
        {
            return Funcao == "Principal" || Funcao == "Assistente";
        }
    }
}
