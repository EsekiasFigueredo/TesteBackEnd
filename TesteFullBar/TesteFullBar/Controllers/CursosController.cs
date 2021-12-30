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
    public class CursosController : Controller
    {
        private readonly TesteFullBarContext _context;

        public CursosController(TesteFullBarContext context)
        {
            _context = context;
        }

        // GET: Cursos
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Cursos.ToListAsync());
        }

        // GET: Cursos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // GET: Cursos/Create
        public IActionResult Create()
        {
            var listaDisciplinas = _context.Disciplinas.ToList();
            ViewBag.Disciplina = new SelectList(listaDisciplinas, "Id", "Nome_Disciplina");
            return View();
        }

        // POST: Cursos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome_Curso,Disciplina_Id")] CursoVM cursoVM)
        {
            Curso curso = new();
            if (ModelState.IsValid)
            {
                curso = new Curso()
                {
                    Nome_Curso = cursoVM.Nome_Curso
                };                
                _context.Add(curso);
                await _context.SaveChangesAsync();
                //buscando curso adicionado
                var cursoAdciconado =  _context.Cursos.Where(o => o.Nome_Curso == cursoVM.Nome_Curso).FirstOrDefault();
                //Adicionado relação entre disciplinas e curso
                if (cursoVM.Disciplina_Id != null)
                {
                    for (int i = 0; i < cursoVM.Disciplina_Id.Count(); i++)
                    {
                        DisciplinaCurso dC = new()
                        {
                           Curso_Id = cursoAdciconado.Id,
                           Disciplina_Id = cursoVM.Disciplina_Id[i]
                        };
                        _context.DisciplinaCursos.Add(dC);
                        _context.SaveChanges();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        // GET: Cursos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var curso = await _context.Cursos.FindAsync(id);
            CursoVM cursoVM = new() 
            { 
                Id = curso.Id,
                Nome_Curso = curso.Nome_Curso
            };
            var disciplinasDoCurso = _context.DisciplinaCursos.ToList();
            var listaDisciplinas = _context.Disciplinas.ToList();
            ViewBag.Disciplina = new MultiSelectList(listaDisciplinas, "Id", "Nome_Disciplina",disciplinasDoCurso.Select(o => o.Disciplina_Id)?.ToList());
            if (curso == null)
            {
                return NotFound();
            }
            return View(cursoVM);
        }

        // POST: Cursos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome_Curso,Disciplina_Id")] CursoVM cursoVM)
        {
            Curso curso = new();
            if (id != cursoVM.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {

                    _context.Cursos.Update(new Curso()
                    {
                        Nome_Curso = cursoVM.Nome_Curso
                    });
                    await _context.SaveChangesAsync();
                    //buscando curso adicionado
                    //var cursoAdciconado = _context.Cursos.Where(o => o.Nome_Curso == cursoVM.Nome_Curso).FirstOrDefault();
                    //Adicionado relação entre disciplinas e curso
                    //if (cursoVM.Disciplina_Id != null)
                    //{
                    //    for (int i = 0; i < cursoVM.Disciplina_Id.Count(); i++)
                    //    {
                    //        DisciplinaCurso dC = new()
                    //        {
                    //            Curso_Id = cursoAdciconado.Id,
                    //            Disciplina_Id = cursoVM.Disciplina_Id[i]
                    //        };
                    //        _context.DisciplinaCursos.Update(dC);
                    //        await _context.SaveChangesAsync();
                    //    }
                    //}
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.Id))
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
            return View(curso);
        }

        // GET: Cursos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Cursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}
