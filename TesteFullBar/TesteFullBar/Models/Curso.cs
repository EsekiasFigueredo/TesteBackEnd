namespace TesteFullBar.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nome_Curso  { get; set; } = string.Empty;
        public int Disciplina_Id { get; set; }

        public virtual ICollection<Disciplina>? Disciplinas { get; set; }
    }
}
