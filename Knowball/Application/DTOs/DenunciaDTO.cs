namespace Knowball.Application.DTOs
{
    public class DenunciaDto
    {
        public int IdDenuncia { get; set; }
        public int IdPartida { get; set; }
        public int IdArbitro { get; set; }
        public string Protocolo { get; set; } = string.Empty;
        public string Relato { get; set; } = string.Empty;
        public DateTime DataDenuncia { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ResultadoAnalise { get; set; } = string.Empty;
    }
}
