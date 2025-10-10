namespace Knowball.Application.DTOs
{
    public class ArbitroDto
    {
        public int IdArbitro { get; set; }
        public string Nome { get; set; } = string.Empty;
        
        public DateTime? DataNascimento { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
