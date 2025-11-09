namespace Knowball.Application.DTOs
{
    public class PartidaDto
    {
        public int IdPartida { get; set; }
        public int IdCampeonato { get; set; }
        public int IdEquipe1 { get; set; }
        public int IdEquipe2 { get; set; }
        public DateTime DataPartida { get; set; }
        public string Local { get; set; } = string.Empty;
        public int PlacarMandante { get; set; }
        public int PlacarVisitante { get; set; }
    }
}