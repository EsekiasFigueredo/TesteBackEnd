#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TesteFullBar.Data;
using TesteFullBar.Models;
using TesteFullBar.VMs;

namespace TesteFullBar.Controllers
{
    public class AlunosController : Controller
    {
        private readonly TesteFullBarContext _context;
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
        public AlunosController(TesteFullBarContext context)
        {
            _context = context;
        }

        // GET: Alunos
        public  IActionResult Index()
        {
            var alunos = _context.Aluno.AsQueryable();
            
            List<AlunoVM> alunoVM = new();
            foreach (var a in alunos)
            {
                alunoVM.Add(new AlunoVM()
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    RA = a.RA,
                    Periodo = a.Periodo,
                    Nome_Curso = NomeCurso(a.Curso_Id),
                    Curso_Id = a.Curso_Id,
                    Status = a.Status
                });
            }

            return View(alunoVM);
        }

        // GET: Alunos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AlunoVM alunoVM = new();
            var aluno = await _context.Aluno.FirstOrDefaultAsync(m => m.Id == id);
            
            alunoVM = new AlunoVM()
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                RA = aluno.RA,
                Periodo = aluno.Periodo,
                Nome_Curso = NomeCurso(aluno.Curso_Id),
                Curso_Id = aluno.Curso_Id,
                Status = aluno.Status
            };

            if (aluno == null)
            {
                return NotFound();
            }

            return View(alunoVM);
        }

        // GET: Alunos/Create
        public IActionResult Create()
        {
            //lista de Curso para Dropdown
            var listaCursos = _context.Cursos.ToList();
            ViewBag.Cursos = new SelectList(listaCursos,"Id", "Nome_Curso");
            //lista Periodos para Dropdown
            var listaPeriodos = this.Periodos.ToList();
            ViewBag.Periodos = new SelectList(listaPeriodos, "Id", "Nome");

            return View();
        }

        // POST: Alunos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,RA,Periodo,Curso_Id,Status,Foto")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aluno);
        }

        // GET: Alunos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Aluno.FindAsync(id);
            //lista de Curso para Dropdown
            var listaCursos = _context.Cursos.ToList();
            ViewBag.Cursos = new SelectList(listaCursos, "Id", "Nome_Curso",aluno.Curso_Id);
            //lista Periodos para Dropdown
            var listaPeriodos = this.Periodos.ToList();
            ViewBag.Periodos = new SelectList(listaPeriodos, "Id", "Nome",aluno.Periodo);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

        // POST: Alunos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,RA,Periodo,Curso_Id,Foto")] Aluno aluno)
        {
            if (id != aluno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aluno);
        }

        // GET: Alunos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Aluno
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // POST: Alunos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aluno = await _context.Aluno.FindAsync(id);
            _context.Aluno.Remove(aluno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(int id)
        {
            return _context.Aluno.Any(e => e.Id == id);
        }
        private string NomeCurso(int cursoId)
        {
            return _context.Cursos.Find(cursoId).Nome_Curso;
        }
    }
}
