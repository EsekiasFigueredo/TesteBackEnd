using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteFullBar.VMs
{

    public class CursoVM
    {
        
        public int Id { get; set; }
        public string Nome_Curso  { get; set; } = string.Empty;
        public List<int>? Disciplina_Id  { get; set; }

    }
}
