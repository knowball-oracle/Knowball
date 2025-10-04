using Knowball.Models;

namespace Knowball.Domain
{
    public class Campeonato
    {
        public int IdCampeonato { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public int Ano { get; set; }

        public bool AnoValido()
        {
            return Ano > 1900 && Ano <= DateTime.Now.Year;
        }

        public bool NomeValido()
        {
            return !string.IsNullOrWhiteSpace(Nome);
        }
    }
}
