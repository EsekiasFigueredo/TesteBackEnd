namespace TesteFullBar.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nome_Curso  { get; set; } = string.Empty;
        public List<int>? Disciplina { get; set; }
    }
}
