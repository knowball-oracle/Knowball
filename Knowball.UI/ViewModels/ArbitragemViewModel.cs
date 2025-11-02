using System.ComponentModel.DataAnnotations;

namespace Knowball.UI.ViewModels
{
    public class ArbitragemViewModel
    {
        public int IdPartida { get; set; }

        [Required(ErrorMessage = "O árbitro é obrigatório")]
        [Display(Name = "Árbitro")]
        public int IdArbitro { get; set; }

        [Required(ErrorMessage = "A função é obrigatória")]
        [StringLength(50, ErrorMessage = "A função deve ter no máximo 50 caracteres")]
        [Display(Name = "Função de Árbitro")]
        public string Funcao { get; set; } = string.Empty;

        public string? NomeArbitro { get; set; }
        public string? DataPartida { get; set; }
        public string? LocalPartida { get; set; }
    }
}
