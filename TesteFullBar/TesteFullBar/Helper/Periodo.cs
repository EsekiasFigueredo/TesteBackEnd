
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
                    new Periodo() { Id = "Manhã", Nome = "Manhã"},
                    new Periodo() { Id = "Tarde", Nome = "Tarde"},
                    new Periodo() { Id = "Noite", Nome = "Noite"}
                };
            }
        }
    }
    
}
