namespace Knowball.Domain
{
    public class Arbitro
    {
        public int IdArbitro { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Status { get; set; }

        public bool CpfValido() => !string.IsNullOrEmpty(Nome);

        public bool StatusValido() =>
            Status == "Ativo" || Status == "Inativo" || Status == "Suspenso";
    }
}
