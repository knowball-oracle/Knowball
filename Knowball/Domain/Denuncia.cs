using System.ComponentModel.DataAnnotations;
using Knowball.Models;

namespace Knowball.Domain
{
    public class Denuncia
    {
        [Key]
        public int IdDenuncia { get; set; }
        public int IdPartida { get; set; }
        public int IdArbitro { get; set; }
        public string Protocolo { get; set; } = string.Empty;
        public string Relato { get; set; } = string.Empty;
        public DateTime DataDenuncia { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ResultadoAnalise { get; set; } = string.Empty;

        public bool StatusValido() => Status == "Em Análise" || Status == "Resolvida";

        public bool ResultadoAnaliseValido() =>
            ResultadoAnalise == null || ResultadoAnalise == "Procedente" || ResultadoAnalise == "Improcedente" || ResultadoAnalise == "Inconclusiva";

        public bool ProtocoloValido() => !string.IsNullOrWhiteSpace(Protocolo);

        public bool RelatoValido() => !string.IsNullOrWhiteSpace(Relato);
    }
}
