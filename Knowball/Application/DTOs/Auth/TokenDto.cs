namespace Fiap.Knowball.Application.DTOs.Auth
{
    public class TokenDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracao { get; set; }
        public string TipoToken { get; set; } = "Bearer";
        }
}
