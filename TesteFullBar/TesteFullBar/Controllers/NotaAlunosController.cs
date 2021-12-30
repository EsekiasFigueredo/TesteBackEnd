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
    public class NotaAlunosController : Controller
    {
        private readonly TesteFullBarContext _context;

        public NotaAlunosController(TesteFullBarContext context)
        {
            _context = context;
        }

        // GET: NotaAlunos
        public async Task<IActionResult> Index(int id)
        {
            var listaNotas = from m in await _context.NotaAluno.ToListAsync()
                             select m;
            listaNotas.Where(o => o.Id_Aluno == id);
            ViewBag.IdAluno = id;
            ViewBag.NomeAluno =  _context.Aluno.Find(id).Nome;
            List<NotasVM> notasVM = new();
            foreach (var n in listaNotas)
            {
                notasVM.Add(new NotasVM() 
                { 
                    Id = n.Id,
                    Id_Aluno = n.Id_Aluno,
                    Nome_Disciplina = _context.Disciplinas.Find(n.Id_Disciplina).Nome_Disciplina,
                    Nota = n.Nota
                });
            }
            return View(notasVM);
        }

        // GET: NotaAlunos/Create
        public IActionResult Create(int id)
        {
            ViewBag.IdAluno = id;
            ViewBag.NomeAluno = _context.Aluno.Find(id).Nome;
            var idCurso = _context.Aluno.Find(id).Curso_Id;
            var disciplinas = ListaDisciplinas(idCurso);
            ViewBag.Disciplinas = new SelectList(disciplinas,"Id", "Nome_Disciplina"); 

            return View();
        }

        // POST: NotaAlunos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,[Bind("Id_Aluno,Id_Disciplina,Nota")] NotaAluno notaAluno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notaAluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id});
            }
            return View(notaAluno);
        }

        // GET: NotaAlunos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Selecinar Nota
            var notaAluno = await _context.NotaAluno.FindAsync(id);
            
            ViewBag.IdAluno = notaAluno.Id_Aluno;
            ViewBag.NomeAluno = _context.Aluno.Find(notaAluno.Id_Aluno).Nome;
            //Preenchimento da ViewBag Disciplina
            var idCurso = _context.Aluno.Find(notaAluno.Id_Aluno).Curso_Id;
            var disciplinas = ListaDisciplinas(idCurso);
            ViewBag.Disciplinas = new SelectList(disciplinas, "Id", "Nome_Disciplina",notaAluno.Id_Disciplina);

            if (notaAluno == null)
            {
                return NotFound();
            }
            return View(notaAluno);
        }

        // POST: NotaAlunos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Id_Aluno,Id_Disciplina,Nota")] NotaAluno notaAluno)
        {
            if (id != notaAluno.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notaAluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaAlunoExists(notaAluno.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = notaAluno.Id_Aluno });
            }
            return View(notaAluno);
        }  

        private bool NotaAlunoExists(int id)
        {
            return _context.NotaAluno.Any(e => e.Id == id);
        }

        public List<Disciplina> ListaDisciplinas(int cursoId)
        {
            List<Disciplina> disc = new();
            var disciplinas = _context.DisciplinaCursos.Where(o => o.Curso_Id == cursoId).AsQueryable();
            var teste = disciplinas.Select(o => o.Disciplina_Id).ToList();
            foreach (var d in teste)
            {
                var Dis = _context.Disciplinas.Find(d);
                disc.Add(new Disciplina()
                {
                    Id = Dis.Id,
                    Nome_Disciplina = Dis.Nome_Disciplina
                });
            }
            return disc;
        }
    }
}
