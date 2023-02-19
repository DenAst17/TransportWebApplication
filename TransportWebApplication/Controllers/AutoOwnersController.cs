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
    public class AutoOwnersController : Controller
    {
        private readonly TransportContext _context;

        public AutoOwnersController(TransportContext context)
        {
            _context = context;
        }

        // GET: AutoOwners
        public async Task<IActionResult> Index(int? autoId, string? autoName)
        {
            if (autoId == null)
            {
                return RedirectToAction("Index", "Models");
            }
            ViewBag.AutoId = autoId; // Just storing data to use it on view
            ViewBag.ModelName = autoName;

            var autoOwners = _context.AutoOwners.Where(ao => ao.AutoId == autoId).Include(ao => ao.Owner);
            
            // var transportContext = _context.AutoOwners.Include(a => a.Auto).Include(a => a.Owner);

            return View(await autoOwners.ToListAsync());
        }

        // GET: AutoOwners/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.AutoOwners == null)
            {
                return NotFound();
            }

            var autoOwner = await _context.AutoOwners
                .Include(a => a.Auto)
                .Include(a => a.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autoOwner == null)
            {
                return NotFound();
            }

            return View(autoOwner);
        }

        // GET: AutoOwners/Create
        public IActionResult Create()
        {
            ViewData["AutoId"] = new SelectList(_context.Autos, "Id", "Vin");
            ViewData["OwnerId"] = new SelectList(_context.Owners, "Id", "Name");
            return View();
        }

        // POST: AutoOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AutoId,OwnerId,StartDate,EndDate,IncidentsInfo")] AutoOwner autoOwner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autoOwner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutoId"] = new SelectList(_context.Autos, "Id", "Id", autoOwner.AutoId);
            ViewData["OwnerId"] = new SelectList(_context.Owners, "Id", "Id", autoOwner.OwnerId);
            return View(autoOwner);
        }

        // GET: AutoOwners/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.AutoOwners == null)
            {
                return NotFound();
            }

            var autoOwner = await _context.AutoOwners.FindAsync(id);
            if (autoOwner == null)
            {
                return NotFound();
            }
            ViewData["AutoId"] = new SelectList(_context.Autos, "Id", "Id", autoOwner.AutoId);
            ViewData["OwnerId"] = new SelectList(_context.Owners, "Id", "Id", autoOwner.OwnerId);
            return View(autoOwner);
        }

        // POST: AutoOwners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,AutoId,OwnerId,StartDate,EndDate,IncidentsInfo")] AutoOwner autoOwner)
        {
            if (id != autoOwner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autoOwner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutoOwnerExists(autoOwner.Id))
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
            ViewData["AutoId"] = new SelectList(_context.Autos, "Id", "Id", autoOwner.AutoId);
            ViewData["OwnerId"] = new SelectList(_context.Owners, "Id", "Id", autoOwner.OwnerId);
            return View(autoOwner);
        }

        // GET: AutoOwners/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.AutoOwners == null)
            {
                return NotFound();
            }

            var autoOwner = await _context.AutoOwners
                .Include(a => a.Auto)
                .Include(a => a.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autoOwner == null)
            {
                return NotFound();
            }

            return View(autoOwner);
        }

        // POST: AutoOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.AutoOwners == null)
            {
                return Problem("Entity set 'TransportContext.AutoOwners'  is null.");
            }
            var autoOwner = await _context.AutoOwners.FindAsync(id);
            if (autoOwner != null)
            {
                _context.AutoOwners.Remove(autoOwner);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutoOwnerExists(long id)
        {
          return _context.AutoOwners.Any(e => e.Id == id);
        }
    }
}
