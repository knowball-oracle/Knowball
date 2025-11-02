using System.ComponentModel.DataAnnotations;

namespace Knowball.UI.ViewModels
{
    public class ParticipacaoViewModel
    {

        [Required(ErrorMessage = "A partida é obrigatória")]
        [Display(Name = "Partida")]
        public int IdPartida { get; set; }

        [Required(ErrorMessage = "A equipe é obrigatória")]
        [Display(Name = "Equipe")]
        public int IdEquipe { get; set; }

        [Required(ErrorMessage = "O tipo de participação é obrigatório")]
        [Display(Name = "Tipo de Participação")]
        public string Tipo { get; set; } = string.Empty;

        public string? NomeEquipe { get; set; }
        public string? DataPartida { get; set; }
        public string? LocalPartida { get; set; }
        public string? CidadeEquipe { get; set; }
    }
}
