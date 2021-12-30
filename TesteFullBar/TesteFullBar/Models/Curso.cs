﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteFullBar.Models
{

    public class Curso
    {
        [Key]
        public int Id { get; set; }
        public string Nome_Curso  { get; set; } = string.Empty;

    }
}
