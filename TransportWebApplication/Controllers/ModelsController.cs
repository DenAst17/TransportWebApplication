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
    public class ModelsController : Controller
    {
        private readonly TransportContext _context;

        public ModelsController(TransportContext context)
        {
            _context = context;
        }

        // GET: Models
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
              return _context.Models != null ? 
                          View(await _context.Models.ToListAsync(cancellationToken)) :
                          Problem("Entity set 'TransportContext.Models'  is null.");
        }

        // GET: Models/Details/5
        public async Task<IActionResult> Details(long? id, CancellationToken cancellationToken)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
            if (model == null)
            {
                return NotFound();
            }

            //return View(model);

            return RedirectToAction("Index", "Autos", new { id = model.Id, name = model.Name});
        }

        // GET: Models/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Models/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Model model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(model, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Models/Edit/5
        public async Task<IActionResult> Edit(long? id, CancellationToken cancellationToken)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }

            var model = await _context.Models.FindAsync(new object[] { id }, cancellationToken);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Models/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] Model model, CancellationToken cancellationToken)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.Id))
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
            return View(model);
        }

        // GET: Models/Delete/5
        public async Task<IActionResult> Delete(long? id, CancellationToken cancellationToken)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Models/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, CancellationToken cancellationToken)
        {
            if (_context.Models == null)
            {
                return Problem("Entity set 'TransportContext.Models'  is null.");
            }
            var model = await _context.Models.FindAsync(new object[] { id }, cancellationToken);
            if (model != null)
            {
                _context.Models.Remove(model);
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(long id)
        {
          return (_context.Models?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
