namespace Knowball.Domain
{
    public class Arbitro
    {
        public int IdArbitro { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Status { get; set; }

        public bool CpfValido()
        {
            return !string.IsNullOrWhiteSpace(Cpf) && Cpf.Length == 11 && Cpf.All(Char.IsDigit);
        }

        public bool NomeValido()
        {
            return !string.IsNullOrWhiteSpace(Nome);
        }
    }
}
