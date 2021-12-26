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
    }
}
