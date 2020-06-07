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
            var eParkingContext = _context.ParkingLokacija.Include(p => p.Cjenovnik).Include(p => p.Vlasnik);
            return View(await eParkingContext.ToListAsync());
        }

        // GET: ParkingLokacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLokacija = await _context.ParkingLokacija
                .Include(p => p.Cjenovnik)
                .Include(p => p.Vlasnik)
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
            //ViewData["CjenovnikId"] = new SelectList(_context.Cjenovnik, "ID", "Naziv");
            ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime");
            return View();
        }

        // POST: ParkingLokacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Adresa,Lat,Long,Kapacitet,BrojSlobodnihMjesta,CjenovnikId,VlasnikId")] ParkingLokacija parkingLokacija)
        {
            if (ModelState.IsValid)
            {
                Cjenovnik cj = Newtonsoft.Json.JsonConvert.DeserializeObject<Cjenovnik>((string)TempData["cjenovnik"]);
                parkingLokacija.CjenovnikId = cj.ID;
                parkingLokacija.BrojSlobodnihMjesta = parkingLokacija.Kapacitet;
                _context.Add(parkingLokacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CjenovnikId"] = new SelectList(_context.Cjenovnik, "ID", "Naziv", parkingLokacija.CjenovnikId);
            ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime", parkingLokacija.VlasnikId);
            return View(parkingLokacija);
        }

        // GET: ParkingLokacija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLokacija = await _context.ParkingLokacija.FindAsync(id);
            if (parkingLokacija == null)
            {
                return NotFound();
            }
            ViewData["CjenovnikId"] = new SelectList(_context.Cjenovnik, "ID", "Naziv", parkingLokacija.CjenovnikId);
            ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime", parkingLokacija.VlasnikId);
            return View(parkingLokacija);
        }

        // POST: ParkingLokacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Naziv,Adresa,Lat,Long,Kapacitet,BrojSlobodnihMjesta,CjenovnikId,VlasnikId")] ParkingLokacija parkingLokacija)
        {
            parkingLokacija.ID = id;
            if (id != parkingLokacija.ID)
            {
                return NotFound();
            }

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
            ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime", parkingLokacija.VlasnikId);
            return View(parkingLokacija);
        }

        // GET: ParkingLokacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLokacija = await _context.ParkingLokacija
                .Include(p => p.Cjenovnik)
                .Include(p => p.Vlasnik)
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
            var parkingLokacija = await _context.ParkingLokacija.FindAsync(id);
            _context.ParkingLokacija.Remove(parkingLokacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingLokacijaExists(int id)
        {
            return _context.ParkingLokacija.Any(e => e.ID == id);
        }
    }
}
