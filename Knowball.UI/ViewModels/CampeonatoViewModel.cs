using System.ComponentModel.DataAnnotations;

namespace Knowball.UI.ViewModels
{
    public class CampeonatoViewModel
    {
        public int IdCampeonato { get; set; }

        [Required(ErrorMessage = "O nome do campeonato é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome do Campeonato")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ano de início é obrigatório")]
        [Range(1900, 2100, ErrorMessage = "Ano inválido")]
        [Display(Name = "Ano de Início")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatório")]
        public string Categoria { get; set; } = string.Empty;
    }
}
