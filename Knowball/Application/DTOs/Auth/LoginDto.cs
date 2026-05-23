using System.ComponentModel.DataAnnotations;

namespace Fiap.Knowball.Application.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;
    }
}
