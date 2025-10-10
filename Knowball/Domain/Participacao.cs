using System.ComponentModel.DataAnnotations;
using Knowball.Models;

namespace Knowball.Domain
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
