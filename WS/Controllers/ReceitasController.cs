using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WS.Context;
using WS.Models;
using WS.Models.ViewModels;

namespace WS.Controllers
{
    public class ReceitasController : Controller
    {
        private readonly AppDbContext _context;

        public ReceitasController(AppDbContext context)
        {
            _context = context;
        }

        private static List<ReceitaViewModel> receitas = new List<ReceitaViewModel>();

        // GET: Receitas
        public IActionResult Index()
        {
            TimeSpan horaAtual = DateTime.Now.TimeOfDay;
            var appDbContext =  _context.Receitas.Include(r => r.Paciente).AsEnumerable()
                .OrderBy(r => Math.Abs((Convert.ToDateTime(r.Hora).TimeOfDay - horaAtual).TotalMinutes));
            return View( appDbContext.ToList());
        }

        // GET: Receitas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas
                .Include(r => r.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receita == null)
            {
                return NotFound();
            }

            return View(receita);
        }





        // GET: Receitas/Create
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Nome", "Nome");
            //return View();
            
            ViewData["Receitas"] = receitas; // Passa a lista para a view
            return View();
        }

        [HttpPost]
        public IActionResult AdicionarReceita(ReceitaViewModel receita)
        {
            if (ModelState.IsValid)
            {
                receitas.Add(receita);
            }

            // Retorna a view Create com a lista de receitas atualizada
            ViewBag.Receitas = receitas;
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Nome", "Nome");
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Finalizar()
        {
            foreach (var item in receitas)
            {
                var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Nome == item.NomePaciente);
                if (paciente != null)
                {
                    var receita = new Receita
                    {
                        Remedio = item.NomeRemedio,
                        Hora = item.Horario,
                        PacienteId = paciente.Id
                    };
                    _context.Receitas.Add(receita);
                }
            }
            await _context.SaveChangesAsync();
            receitas.Clear();

            return RedirectToAction("Index");
        }

        // POST: Receitas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Remedio,Hora,PacienteId")] Receita receita)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", receita.PacienteId);
            return View(receita);
        }

        // GET: Receitas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas.FindAsync(id);
            if (receita == null)
            {
                return NotFound();
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", receita.PacienteId);
            return View(receita);
        }

        // POST: Receitas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Remedio,Hora,PacienteId")] Receita receita)
        {
            if (id != receita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceitaExists(receita.Id))
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
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nome", receita.PacienteId);
            return View(receita);
        }

        // GET: Receitas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas
                .Include(r => r.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receita == null)
            {
                return NotFound();
            }

            return View(receita);
        }

        // POST: Receitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receita = await _context.Receitas.FindAsync(id);
            if (receita != null)
            {
                _context.Receitas.Remove(receita);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceitaExists(int id)
        {
            return _context.Receitas.Any(e => e.Id == id);
        }
    }
}
