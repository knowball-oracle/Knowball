namespace Knowball.Application.DTOs
{
    public class DenunciaDTO
    {
        public int IdPartida { get; set; }
        public int IdArbitro { get; set; }
        public string Protocolo { get; set; }
        public string Relato { get; set; }
        public DateTime DataDenuncia { get; set; }
        public string Status { get; set; }
        public string ResultadoAnalise { get; set; }
    }
}
