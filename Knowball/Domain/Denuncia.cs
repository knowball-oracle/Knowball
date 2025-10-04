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

        public bool StatusValido()
        {
            return Status == "Pendente" || Status == "Aprovada" || Status == "Rejeitada";
        }

        public bool ProtocoloValido()
        {
            return !string.IsNullOrWhiteSpace(Protocolo);
        }

        public bool RelatoValido()
        {
            return !string.IsNullOrWhiteSpace(Relato);
        }
    }
}
