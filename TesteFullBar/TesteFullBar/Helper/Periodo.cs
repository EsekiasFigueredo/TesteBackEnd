
namespace TesteFullBar.Helper
{
    public class TipoPeriodo
    {
        public class Periodo
        {
            public string Id { get; set; } = string.Empty;
            public string Nome { get; set; } = string.Empty;
        }
        public IEnumerable<Periodo> Periodos
        {
            get
            {
                return new List<Periodo>()
                {
                    new Periodo() { Id = "1° Periodo", Nome = "1° Periodo"},
                    new Periodo() { Id = "2° Periodo", Nome = "2° Periodo"},
                    new Periodo() { Id = "3° Periodo", Nome = "3° Periodo"},
                    new Periodo() { Id = "4° Periodo", Nome = "4° Periodo"},
                    new Periodo() { Id = "5° Periodo", Nome = "5° Periodo"},
                    new Periodo() { Id = "6° Periodo", Nome = "6° Periodo"},
                    new Periodo() { Id = "7° Periodo", Nome = "7° Periodo"},
                    new Periodo() { Id = "8° Periodo", Nome = "8° Periodo"},
                    new Periodo() { Id = "9° Periodo", Nome = "9° Periodo"},
                    new Periodo() { Id = "10° Periodo", Nome = "10° Periodo"},
                  
                };
            }
        }
    }
    
}
