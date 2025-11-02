using System.ComponentModel.DataAnnotations;

namespace Knowball.UI.ViewModels
{
    public class DenunciaViewModel
    {
        public int IdDenuncia { get; set; }

        [Required(ErrorMessage = "O protocolo é obrigatório")]
        [StringLength(50, ErrorMessage = "O protocolo deve ter no máximo 50 caracteres")]
        [Display(Name = "Número do Protocolo")]
        public string Protocolo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O relato é obrigatório")]
        [StringLength(500, MinimumLength = 10,
            ErrorMessage = "O relato deve ter entre 10 e 500 caracteres")]
        [Display(Name = "Relato da Denúncia")]
        public string Relato { get; set; } = string.Empty;

        [Required(ErrorMessage = "O status é obrigatório")]
        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "O resultado deve ter no máximo 500 caracteres")]
        [Display(Name = "Resultado da Análise")]
        public string ResultadoAnalise { get; set; } = string.Empty;

        [Required(ErrorMessage = "A partida é obrigatória")]
        [Display(Name = "Partida")]
        public int IdPartida { get; set; }

        [Required(ErrorMessage = "O árbitro é obrigatório")]
        [Display(Name = "Árbitro")]
        public int IdArbitro { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Denúncia")]
        public DateTime? DataDenuncia { get; set; }

        public string? LocalPartida { get; set; }
        public string? DataPartida { get; set; }
        public string? CampeonatoPartida { get; set; }
        public string? NomeArbitro { get; set; }
    }
}
