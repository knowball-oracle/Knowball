namespace Knowball.Domain
{
    public class Arbitro
    {
        public int IdArbitro { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Status { get; set; }

        public ICollection<Arbitragem> Arbitragens { get; set; }

        public Arbitro()
        {
            Arbitragens = new List<Arbitragem>();
        }
    }
}
