using Knowball.Models;

namespace Knowball.Domain
{
    public class Denuncia
    {
        public int IdDenuncia { get; set; }
        public int IdPartida { get; set; }
        public string Protocolo { get; set; }
        public string Relato { get; set; }
        public DateTime DataHora { get; set; }
        public string Sentimento { get; set; }
        public string Status { get; set; }

        public Partida Partida { get; set; }
    }
}
