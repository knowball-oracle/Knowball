using Knowball.Models;

namespace Knowball.Domain
{
    public class Participacao
    {
        public int IdPartida { get; set; }
        public int IdEquipe { get; set; }
        public string Tipo { get; set; }

        public bool TipoValido() => Tipo == "Mandante" || Tipo == "Visitante";
    }
}
