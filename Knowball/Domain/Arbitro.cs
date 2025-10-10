using System.ComponentModel.DataAnnotations;

namespace Knowball.Domain
{
    public class Arbitro
    {
        [Key]
        public int IdArbitro { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public string Status { get; set; } = string.Empty;

        public bool CpfValido() => !string.IsNullOrEmpty(Nome);

        public bool StatusValido() =>
            Status == "Ativo" || Status == "Inativo" || Status == "Suspenso";
    }
}
