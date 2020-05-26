using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EParking.Models;

namespace EParking.Controllers
{
    public class ParkingLokacijaController : Controller
    {
        private readonly EParkingContext _context;

        public ParkingLokacijaController(EParkingContext context)
        {
            _context = context;
        }

        // GET: ParkingLokacija
        public async Task<IActionResult> Index()
        {
            var eParkingContext = _context.ParkingLokacija.Include(p => p.Cjenovnik);
            return View(await eParkingContext.ToListAsync());
        }

        // GET: ParkingLokacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //id -= 1;

            var parkingLokacija = await _context.ParkingLokacija
                .Include(p => p.Cjenovnik)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (parkingLokacija == null)
            {
                return NotFound();
            }

            return View(parkingLokacija);
        }

        // GET: ParkingLokacija/Create
        public IActionResult Create()
        {
            ViewData["CjenovnikId"] = new SelectList(_context.Cjenovnik, "ID", "Naziv");
            return View();
        }

        // POST: ParkingLokacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Adresa,Lat,Long,Kapacitet,BrojSlobodnihMjesta,CjenovnikId")] ParkingLokacija parkingLokacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkingLokacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CjenovnikId"] = new SelectList(_context.Cjenovnik, "ID", "Naziv", parkingLokacija.CjenovnikId);
            return View(parkingLokacija);
        }

        // GET: ParkingLokacija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //id -= 1;

            var parkingLokacija = await _context.ParkingLokacija.FindAsync(id);
            if (parkingLokacija == null)
            {
                return NotFound();
            }
            ViewData["CjenovnikId"] = new SelectList(_context.Cjenovnik, "ID", "Naziv", parkingLokacija.CjenovnikId);
            return View(parkingLokacija);
        }

        // POST: ParkingLokacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Naziv,Adresa,Lat,Long,Kapacitet,BrojSlobodnihMjesta,CjenovnikId")] ParkingLokacija parkingLokacija)
        {
            //dodano jer nije radio edit
            parkingLokacija.ID = id;
            if (id != parkingLokacija.ID)
            {
                return NotFound();
            }

            //id -= 1;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingLokacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingLokacijaExists(parkingLokacija.ID))
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
            ViewData["CjenovnikId"] = new SelectList(_context.Cjenovnik, "ID", "Naziv", parkingLokacija.CjenovnikId);
            return View(parkingLokacija);
        }

        // GET: ParkingLokacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //id -= 1;

            var parkingLokacija = await _context.ParkingLokacija
                .Include(p => p.Cjenovnik)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (parkingLokacija == null)
            {
                return NotFound();
            }

            return View(parkingLokacija);
        }

        // POST: ParkingLokacija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //id -= 1;
            var parkingLokacija = await _context.ParkingLokacija.FindAsync(id);
            _context.ParkingLokacija.Remove(parkingLokacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingLokacijaExists(int id)
        {
            //id -= 1;
            return _context.ParkingLokacija.Any(e => e.ID == id);
        }
    }
}
