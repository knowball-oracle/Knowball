using System.ComponentModel.DataAnnotations;

namespace Knowball.UI.ViewModels
{
    public class EquipeViewModel
    {
        public int IdEquipe { get; set; }

        [Required(ErrorMessage = "O nome da equipe é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome da Equipe")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória")]
        [StringLength(50, ErrorMessage = "A cidade deve ter no máximo 50 caracteres")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Use a sigla do estado (2 letras)")]
        [Display(Name = "Estado (UF)")]
        public string Estado { get; set; } = string.Empty;
    }
}
