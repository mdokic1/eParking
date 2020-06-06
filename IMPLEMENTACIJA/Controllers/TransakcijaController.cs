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
    public class TransakcijaController : Controller
    {
        private readonly EParkingContext _context;

        public TransakcijaController(EParkingContext context)
        {
            _context = context;
        }

        // GET: Transakcija
        public async Task<IActionResult> Index()
        {
            var eParkingContext = _context.Transakcija.Include(t => t.ParkingLokacija).Include(t => t.Vozilo);
            return View(await eParkingContext.ToListAsync());
        }

        // GET: Transakcija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transakcija = await _context.Transakcija
                .Include(t => t.ParkingLokacija)
                .Include(t => t.Vozilo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (transakcija == null)
            {
                return NotFound();
            }

            return View(transakcija);
        }

        // GET: Transakcija/Create
        public IActionResult Create()
        {
            ViewData["ParkingLokacijaId"] = new SelectList(_context.ParkingLokacija, "ID", "Adresa");
            ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora");
            return View();
        }

        // POST: Transakcija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VrijemeDolaska,VrijemeOdlaska,Iznos,ParkingLokacijaId,VoziloId")] Transakcija transakcija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transakcija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParkingLokacijaId"] = new SelectList(_context.ParkingLokacija, "ID", "Adresa", transakcija.ParkingLokacijaId);
            ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora", transakcija.VoziloId);
            return View(transakcija);
        }

        // GET: Transakcija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transakcija = await _context.Transakcija.FindAsync(id);
            if (transakcija == null)
            {
                return NotFound();
            }
            ViewData["ParkingLokacijaId"] = new SelectList(_context.ParkingLokacija, "ID", "Adresa", transakcija.ParkingLokacijaId);
            ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora", transakcija.VoziloId);
            return View(transakcija);
        }

        // POST: Transakcija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VrijemeDolaska,VrijemeOdlaska,Iznos,ParkingLokacijaId,VoziloId")] Transakcija transakcija)
        {
            if (id != transakcija.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transakcija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransakcijaExists(transakcija.ID))
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
            ViewData["ParkingLokacijaId"] = new SelectList(_context.ParkingLokacija, "ID", "Adresa", transakcija.ParkingLokacijaId);
            ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora", transakcija.VoziloId);
            return View(transakcija);
        }

        // GET: Transakcija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transakcija = await _context.Transakcija
                .Include(t => t.ParkingLokacija)
                .Include(t => t.Vozilo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (transakcija == null)
            {
                return NotFound();
            }

            return View(transakcija);
        }

        // POST: Transakcija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transakcija = await _context.Transakcija.FindAsync(id);
            _context.Transakcija.Remove(transakcija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransakcijaExists(int id)
        {
            return _context.Transakcija.Any(e => e.ID == id);
        }
    }
}
