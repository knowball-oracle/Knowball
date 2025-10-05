namespace Knowball.Application.DTOs
{
    public class PartidaDTO
    {
        public int IdCampeonato { get; set; }
        public DateTime DataPartida { get; set; }
        public string Local { get; set; }
        public int PlacarMandante { get; set; }
        public int PlacarVisitante { get; set; }
    }
}
