using System.ComponentModel.DataAnnotations;

namespace Knowball.Domain
{
    public class Equipe
    {
        [Key]
        public int IdEquipe { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        public bool DadosValidos() =>
            !string.IsNullOrWhiteSpace(Nome) &&
            !string.IsNullOrWhiteSpace(Estado) &&
            Estado.Length == 2;
    }
}
