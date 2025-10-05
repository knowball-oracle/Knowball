namespace Knowball.Domain
{
    public class Equipe
    {
        public int IdEquipe { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public bool DadosValidos() =>
            !string.IsNullOrWhiteSpace(Nome) &&
            !string.IsNullOrWhiteSpace(Estado) &&
            Estado.Length == 2;
    }
}
