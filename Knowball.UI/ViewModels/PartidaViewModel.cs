using System.ComponentModel.DataAnnotations;

namespace Knowball.UI.ViewModels
{
    public class PartidaViewModel
    {
        public int IdPartida { get; set; }

        [Required(ErrorMessage = "O campeonato é obrigatório")]
        [Display(Name = "Campeonato")]
        public int IdCampeonato { get; set; }

        [Required(ErrorMessage = "A equipe mandante é obrigatória")]
        [Display(Name = "Equipe Mandante")]
        public int IdEquipe1 { get; set; }

        [Required(ErrorMessage = "A equipe visitante é obrigatória")]
        [Display(Name = "Equipe Visitante")]
        public int IdEquipe2 { get; set; }

        [Required(ErrorMessage = "A data e hora são obrigatórias")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data e Hora")]
        public DateTime DataPartida { get; set; }

        [Required(ErrorMessage = "O local é obrigatório")]
        [StringLength(100, ErrorMessage = "O local deve ter no máximo 100 caracteres")]
        [Display(Name = "Local")]
        public string Local { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "O placar não pode ser negativo")]
        [Display(Name = "Placar Mandante")]
        public int PlacarMandante { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O placar não pode ser negativo")]
        [Display(Name = "Placar Visitante")]
        public int PlacarVisitante { get; set; }

        public string? NomeCampeonato { get; set; }
        public string? NomeEquipe1 { get; set; }
        public string? NomeEquipe2 { get; set; }
    }
}
