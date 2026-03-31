using System.ComponentModel.DataAnnotations;

namespace Fiap.Knowball.Domain
{
    public class Participacao
    {
        [Key]
        public int IdPartida { get; set; }
        public int IdEquipe { get; set; }
        public string Tipo { get; set; } = string.Empty;

        public bool TipoValido() => Tipo == "Mandante" || Tipo == "Visitante";
    }
}
