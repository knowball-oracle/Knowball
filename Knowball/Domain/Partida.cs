using Knowball.Domain;

namespace Knowball.Models
{
    public class Partida
    {
        public int IdPartida { get; set; }
        public int IdCampeonato { get; set; }
        public DateTime DataPartida { get; set; }
        public string Local { get; set; }
        public int PlacarMandante { get; set; }
        public int PlacarVisitante { get; set; }
        public ICollection<Participacao> Participacoes { get; set; } = new List<Participacao>();

        public bool PlacarValido()
        {
            //Regra: placares não podem ser menores que zero
            return PlacarMandante >= 0 && PlacarVisitante >= 0;
        }

        public bool DataValida()
        {
            return DataPartida >= DateTime.Today;
        }

        public bool TemDuasEquipes()
        {
            return Participacoes != null && Participacoes.Count == 2;
        }
    }
}
