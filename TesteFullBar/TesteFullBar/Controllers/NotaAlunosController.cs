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
        public async Task<IActionResult> Index()
        {
            return View(await _context.NotaAluno.ToListAsync());
        }

        // GET: NotaAlunos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaAluno = await _context.NotaAluno
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notaAluno == null)
            {
                return NotFound();
            }

            return View(notaAluno);
        }

        // GET: NotaAlunos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotaAlunos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Id_Aluno,Id_Disciplina,Nota")] NotaAluno notaAluno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notaAluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

            var notaAluno = await _context.NotaAluno.FindAsync(id);
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
                return RedirectToAction(nameof(Index));
            }
            return View(notaAluno);
        }

        // GET: NotaAlunos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaAluno = await _context.NotaAluno
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notaAluno == null)
            {
                return NotFound();
            }

            return View(notaAluno);
        }

        // POST: NotaAlunos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notaAluno = await _context.NotaAluno.FindAsync(id);
            _context.NotaAluno.Remove(notaAluno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotaAlunoExists(int id)
        {
            return _context.NotaAluno.Any(e => e.Id == id);
        }
    }
}
