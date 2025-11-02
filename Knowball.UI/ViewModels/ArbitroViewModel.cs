using System.ComponentModel.DataAnnotations;

namespace Knowball.UI.ViewModels
{
    public class ArbitroViewModel
    {
        public int IdArbitro { get; set; }

        [Required(ErrorMessage = "O nome do árbitro é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome do Árbitro")]
        public string Nome { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "O status é obrigatório")]
        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;
    }
}
