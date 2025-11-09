using System.ComponentModel.DataAnnotations;
using Knowball.Domain;

namespace Knowball.Models
{
    public class Partida
    {
        [Key]
        public int IdPartida { get; set; }
        public int IdCampeonato { get; set; }
        public DateTime DataPartida { get; set; }
        public string Local { get; set; } = string.Empty;
        public int PlacarMandante { get; set; }
        public int PlacarVisitante { get; set; }

        public ICollection<Participacao>? Participacoes { get; set; }

        public bool PlacarValido() => PlacarMandante >= 0 && PlacarVisitante >= 0;

        public bool DataValida() => DataPartida >= DateTime.Today;
    }
}