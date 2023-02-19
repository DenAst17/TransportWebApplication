using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TransportWebApplication.Models;

namespace TransportWebApplication.Controllers
{
    public class AutosController : Controller
    {
        private readonly TransportContext _context;

        public AutosController(TransportContext context)
        {
            _context = context;
        }

        // GET: Autos
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Models");
            }
            ViewBag.ModelId = id; // Just storing data to use it on view
            ViewBag.ModelName = name;

            var autosByModel = _context.Autos.Where(a => a.ModelId == id).Include(a => a.Model);

            return View(await autosByModel.ToListAsync());
        }

        // GET: Autos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Autos == null)
            {
                return NotFound();
            }

            var auto = await _context.Autos
                .Include(a => a.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auto == null)
            {
                return NotFound();
            }

            return View(auto);
        }

        // GET: Autos/Create
        public IActionResult Create(int modelId)
        {
            //ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name");
            ViewBag.ModelId = modelId;
            ViewBag.ModelName = _context.Models.Where(m => m.Id == modelId).FirstOrDefault().Name;
            Console.WriteLine("..");
            Console.WriteLine(ViewBag.ModelName);
            Console.WriteLine("..");
            return View();
        }

        // POST: Autos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ModelId, [Bind("Id,Color,Vin,RegisterCode")] Auto auto)
        {
            auto.ModelId = ModelId;
            if (ModelState.IsValid)
            {
                _context.Add(auto);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Autos", new { id = ModelId, name = _context.Models.Where(m => m.Id == ModelId).FirstOrDefault().Name });
            }
            //ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", auto.ModelId);
            //return View(auto);
            return RedirectToAction("Index", "Autos", new { id = ModelId, name = _context.Models.Where(m => m.Id == ModelId).FirstOrDefault().Name });

        }

            // GET: Autos/Edit/5
            public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Autos == null)
            {
                return NotFound();
            }

            var auto = await _context.Autos.FindAsync(id);
            if (auto == null)
            {
                return NotFound();
            }
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", auto.ModelId);
            return View(auto);
        }

        // POST: Autos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ModelId,Color,Vin,RegisterCode")] Auto auto)
        {
            if (id != auto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutoExists(auto.Id))
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
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Name", auto.ModelId);
            return View(auto);
        }

        // GET: Autos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Autos == null)
            {
                return NotFound();
            }

            var auto = await _context.Autos
                .Include(a => a.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auto == null)
            {
                return NotFound();
            }

            return View(auto);
        }

        // POST: Autos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Autos == null)
            {
                return Problem("Entity set 'TransportContext.Autos'  is null.");
            }
            var auto = await _context.Autos.FindAsync(id);
            if (auto != null)
            {
                _context.Autos.Remove(auto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutoExists(long id)
        {
          return (_context.Autos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
