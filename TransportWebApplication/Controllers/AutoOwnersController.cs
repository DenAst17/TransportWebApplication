using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TransportWebApplication.Exceptions;
using TransportWebApplication.Models;
using TransportWebApplication.Services;

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
        public async Task<IActionResult> Index(int? autoId, string? autoName, CancellationToken cancellationToken)
        {
            if (autoId == null)
            {
                return RedirectToAction("Index", "Models");
            }
            ViewBag.AutoId = autoId; // Just storing data to use it on view
            ViewBag.AutoName = autoName;

            var autoOwners = await _context.AutoOwners
                .Where(ao => ao.AutoId == autoId)
                .Include(ao => ao.Owner)
                .ToListAsync(cancellationToken);

            return View(autoOwners);
        }

        // GET: AutoOwners/Details/5
        public async Task<IActionResult> Details(long? id, CancellationToken cancellationToken)
        {
            if (id == null || _context.AutoOwners == null)
            {
                return NotFound();
            }

            var autoOwner = await _context.AutoOwners
                .Include(a => a.Auto)
                .Include(a => a.Owner)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (autoOwner == null)
            {
                return NotFound();
            }

            return View(autoOwner);
        }

        // GET: AutoOwners/Create
        public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
        {
            /*Console.WriteLine(id);
            Console.WriteLine(name);*/
            var auto = await _context.Autos.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if(auto is null)
            {
                return NotFound();
            }

            var owners = await _context.Owners.ToListAsync(cancellationToken);

            ViewData["OwnerId"] = new SelectList(owners, "Id", "Name");
            ViewData["Auto"] = auto;


            var autoOwner = new AutoOwner
            {
                Auto = auto
            };

            return View(autoOwner);
        }

        // POST: AutoOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int AutoId, [Bind("OwnerId,StartDate,EndDate,IncidentsInfo,IsFinite")] AutoOwner autoOwner, CancellationToken cancellationToken)
        {
            autoOwner.AutoId = AutoId;

            OwnershipService ownershipService = new(_context);

            try
            {
                await ownershipService.CreateOwnership(autoOwner, cancellationToken);
            }
            catch (OwnershipException)
            {
                var auto = await _context.Autos.FirstOrDefaultAsync(a => a.Id == AutoId, cancellationToken);
                var owners = await _context.Owners.ToListAsync(cancellationToken);

                ViewData["OwnerId"] = new SelectList(owners, "Id", "Name");

                autoOwner.Auto = auto;
                return View(autoOwner);
            }

            return RedirectToAction("Index", "AutoOwners", new { autoId = AutoId, autoName = _context.Autos.Where(a => a.Id == AutoId).FirstOrDefault().Vin });
        }

        // GET: AutoOwners/Edit/5
        public async Task<IActionResult> Edit(long? id, CancellationToken cancellationToken)
        {
            if (id == null || _context.AutoOwners == null)
            {
                return NotFound();
            }

            var autoOwner = await _context.AutoOwners.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
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
        public async Task<IActionResult> Edit(long id, [Bind("Id,AutoId,OwnerId,StartDate,EndDate,IncidentsInfo")] AutoOwner autoOwner, CancellationToken cancellationToken)
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
                    await _context.SaveChangesAsync(cancellationToken);
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
        public async Task<IActionResult> Delete(long? id, CancellationToken cancellationToken)
        {
            if (id == null || _context.AutoOwners == null)
            {
                return NotFound();
            }

            var autoOwner = await _context.AutoOwners
                .Include(a => a.Auto)
                .Include(a => a.Owner)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
            if (autoOwner == null)
            {
                return NotFound();
            }

            return View(autoOwner);
        }

        // POST: AutoOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, CancellationToken cancellationToken)
        {
            if (_context.AutoOwners == null)
            {
                return Problem("Entity set 'TransportContext.AutoOwners'  is null.");
            }
            var autoOwner = await _context.AutoOwners.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
            if (autoOwner != null)
            {
                _context.AutoOwners.Remove(autoOwner);
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        private bool AutoOwnerExists(long id)
        {
          return _context.AutoOwners.Any(e => e.Id == id);
        }
    }
}
