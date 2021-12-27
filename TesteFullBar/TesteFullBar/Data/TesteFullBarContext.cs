#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesteFullBar.Models;

namespace TesteFullBar.Data
{
    public class TesteFullBarContext : DbContext
    {
        public TesteFullBarContext (DbContextOptions<TesteFullBarContext> options)
            : base(options)
        {
        }

        public DbSet<TesteFullBar.Models.Aluno> Aluno { get; set; }
        public DbSet<TesteFullBar.Models.Disciplina> Disciplinas { get; set; }
        public DbSet<TesteFullBar.Models.Curso> Cursos { get; set; }
        public DbSet<TesteFullBar.Models.NotaAluno> NotaAluno { get; set; }
    }
}
