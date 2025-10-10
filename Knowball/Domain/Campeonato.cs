using System.ComponentModel.DataAnnotations;
using Knowball.Models;

namespace Knowball.Domain
{
    public class Campeonato
    {
        [Key]
        public int IdCampeonato { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int Ano { get; set; }

        public bool AnoValido() => Ano > 1900 && Ano <= DateTime.Now.Year;

        public bool NomeValido() => !string.IsNullOrWhiteSpace(Nome);
        public bool CategoriaValida() => 
            Categoria == "Sub-13" || Categoria == "Sub-15" || Categoria == "Sub-17" || Categoria == "Sub-20";
    }
}
