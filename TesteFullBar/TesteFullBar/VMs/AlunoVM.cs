using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TesteFullBar.Models;

namespace TesteFullBar.VMs
{

    public class AlunoVM
    {

        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string RA { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public int Curso_Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Nome_Curso { get; set; } = string.Empty;
        public string Foto { get; set; } = string.Empty;
        public List<string> DiciplinasReprovados { get; set; } = new List<string>();

    }
}
