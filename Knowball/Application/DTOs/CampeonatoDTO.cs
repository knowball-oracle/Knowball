namespace Knowball.Application.DTOs
{
    public class CampeonatoDto
    {
        public int IdCampeonato { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int Ano { get; set; }
    }
}
