using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EParking.Models;

namespace EParkingOOAD.Controllers
{
    public class ZahtjevController : Controller
    {
        private readonly EParkingContext _context;

        public ZahtjevController(EParkingContext context)
        {
            _context = context;
        }

        // GET: Zahtjev
        public async Task<IActionResult> Index()
        {
            var eParkingContext = _context.Zahtjev.Include(z => z.Vlasnik).Include(z => z.Vozilo);
            return View(await eParkingContext.ToListAsync());
        }

        // GET: Zahtjev/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zahtjev = await _context.Zahtjev
                .Include(z => z.Vlasnik)
                .Include(z => z.Vozilo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (zahtjev == null)
            {
                return NotFound();
            }

            return View(zahtjev);
        }

        // GET: Zahtjev/Create
        public IActionResult Create()
        {
            ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime");
            ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora");
            return View();
        }

        // POST: Zahtjev/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoziloId,VlasnikId")] Zahtjev zahtjev)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zahtjev);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime", zahtjev.VlasnikId);
            ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora", zahtjev.VoziloId);
            return View(zahtjev);
        }

        // GET: Zahtjev/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zahtjev = await _context.Zahtjev.FindAsync(id);
            if (zahtjev == null)
            {
                return NotFound();
            }
            ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime", zahtjev.VlasnikId);
            ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora", zahtjev.VoziloId);
            return View(zahtjev);
        }

        // POST: Zahtjev/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VoziloId,VlasnikId")] Zahtjev zahtjev)
        {
            if (id != zahtjev.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zahtjev);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZahtjevExists(zahtjev.ID))
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
            ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime", zahtjev.VlasnikId);
            ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora", zahtjev.VoziloId);
            return View(zahtjev);
        }

        // GET: Zahtjev/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zahtjev = await _context.Zahtjev
                .Include(z => z.Vlasnik)
                .Include(z => z.Vozilo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (zahtjev == null)
            {
                return NotFound();
            }

            return View(zahtjev);
        }

        // POST: Zahtjev/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zahtjev = await _context.Zahtjev.FindAsync(id);
            _context.Zahtjev.Remove(zahtjev);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZahtjevExists(int id)
        {
            return _context.Zahtjev.Any(e => e.ID == id);
        }
    }
}
