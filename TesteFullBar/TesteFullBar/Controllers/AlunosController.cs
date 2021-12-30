#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TesteFullBar.Data;
using TesteFullBar.Logic;
using TesteFullBar.Models;
using TesteFullBar.VMs;

namespace TesteFullBar.Controllers
{
    public class AlunosController : Controller
    {
        private readonly TesteFullBarContext _context;
        private readonly IWebHostEnvironment _webhostingEnvironment;

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
        public AlunosController(TesteFullBarContext context, IWebHostEnvironment webhostingEnvironment)
        {
            _context = context;
            _webhostingEnvironment = webhostingEnvironment;

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
                    Status = AprovadoReprovado(a.Id),
                });
            }

            return View(alunoVM);
        }
        [HttpPost]
        public IActionResult Index(string nome, string ra, string curso)
        {
            var alunos = from m in _context.Aluno select m;

            if (!string.IsNullOrWhiteSpace(nome))
            {
                alunos = alunos.Where(s => s.Nome!.Contains(nome));
            }
            if (!string.IsNullOrWhiteSpace(ra))
            {
                alunos = alunos.Where(s => s.RA!.Contains(ra));
            }
            if (!string.IsNullOrWhiteSpace(curso))
            {
                var IdCurso = from n in _context.Cursos where n.Nome_Curso == curso select n;
                //IdCurso = IdCurso.Where(o => o.Nome_Disciplina == curso);
                var idc = IdCurso.Select(o => o.Id).ToList();
                alunos = alunos.Where(s => s.Curso_Id == idc[0]);
            }
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
                    Status = AprovadoReprovado(a.Id),
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
                Status = AprovadoReprovado(aluno.Id),
                DiciplinasReprovados = DisciplinaReprovado(aluno.Id),
                Foto = aluno.Foto
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
        public async Task<IActionResult> Create(IFormFile image, Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    string webRoopath = _webhostingEnvironment.WebRootPath;
                    var resp = SalvandoImagem(image, aluno.Nome, webRoopath);
                    if (resp == "Erro com a Imagem!")
                    {
                        ModelState.AddModelError("imagem", resp);
                        return View();
                    }
                    aluno.Foto = resp;
                }
                else
                {
                    aluno.Foto = "imagemDefault.png";
                }
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
        public async Task<IActionResult> Edit(int id, IFormFile imagem, Aluno aluno)
        {
            if (id != aluno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imagem != null)
                    {
                        string webRoopath = _webhostingEnvironment.WebRootPath;
                        var resp = SalvandoImagem(imagem, aluno.Nome, webRoopath);
                        if (resp == "Erro com a Imagem!")
                        {
                            ModelState.AddModelError("imagem", resp);
                            return View();
                        }
                        aluno.Foto = resp;
                    }
                    else
                    {
                        aluno.Foto = "imagemDefault.png";
                    }
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
        public string AprovadoReprovado(int IdAluno)
        {
            var notas = _context.NotaAluno.Where(o => o.Id_Aluno == IdAluno).AsQueryable();
            if (notas.Count() > 0)
            {
                notas = notas.Where(x => x.Nota < 7).AsQueryable();
                return notas.Count() > 0 ? "Reprovado" : "Aprovado";
            }
            else
            {
                return "Não Avaliado";
            }
            
        }
        public List<string> DisciplinaReprovado(int IdAluno)
        {
            var notas = _context.NotaAluno.Where(o => o.Id_Aluno == IdAluno && o.Nota < 7).AsQueryable();
            if(notas.Count() > 0)
            {
                var idDisciplina = notas.Select(o => o.Id_Disciplina).ToList();
                List<string> nome_Disciplina = new();

                foreach (var id in idDisciplina)
                {
                    nome_Disciplina.Add(_context.Disciplinas.Find(id).Nome_Disciplina);
                }

                return nome_Disciplina;
            }
            else
            {
                List<string> nome_ = new();
                nome_.Add("Não se Aplica");
                return nome_ ;
            }
            
        }
        public string SalvandoImagem(IFormFile arquivos,string aluno, string webRoopath) 
        {
            string extensao = Path.GetExtension(arquivos.FileName);
            string nomeArquivo = aluno + DateTime.Now.ToString();
            string caractere = @"(?i)[^A-Za-zãõç.0-9]";
            string replacement = "";
            Regex rgx = new Regex(caractere);
            string renomeado = rgx.Replace(nomeArquivo, replacement);
            nomeArquivo = renomeado + extensao;
            string path = Path.Combine(webRoopath, "imgAlunos");
            string rotaArquivo = Path.Combine(path, nomeArquivo);
            try
            {
                using (var fileStream = new FileStream(rotaArquivo, FileMode.Create))
                {
                    arquivos.CopyTo(fileStream);
                }
                return nomeArquivo;
            }
            catch (Exception ex)
            {
                var erro = "Erro com a Imagem!";
                return erro;
            }

        }
    }
}
