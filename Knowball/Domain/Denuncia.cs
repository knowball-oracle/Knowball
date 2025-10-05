using Knowball.Models;

namespace Knowball.Domain
{
    public class Denuncia
    {
        public int IdDenuncia { get; set; }
        public int IdPartida { get; set; }
        public int IdArbitro { get; set; }
        public string Protocolo { get; set; }
        public string Relato { get; set; }
        public DateTime DataDenuncia { get; set; }
        public string Status { get; set; }
        public string ResultadoAnalise { get; set; }

        public bool StatusValido() => Status == "Em Análise" || Status == "Resolvida";

        public bool ResultadoAnaliseValido() =>
            ResultadoAnalise == null || ResultadoAnalise == "Procedente" || ResultadoAnalise == "Improcedente" || ResultadoAnalise == "Inconclusiva";

        public bool ProtocoloValido() => !string.IsNullOrWhiteSpace(Protocolo);

        public bool RelatoValido() => !string.IsNullOrWhiteSpace(Relato);
    }
}
