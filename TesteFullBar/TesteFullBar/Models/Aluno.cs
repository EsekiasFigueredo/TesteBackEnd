namespace TesteFullBar.Models
{
    public class Aluno
    {
        public int Id { get; set; } 
        public string Nome { get; set; } = string.Empty;
        public string RA { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public int Curso_Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Foto { get; set; } = string.Empty;

        //public virtual Curso Curso { get; set; }

    }
}
