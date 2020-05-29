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
    public class VoziloController : Controller
    {
        private readonly EParkingContext _context;

        public VoziloController(EParkingContext context)
        {
            _context = context;
        }

        // GET: Vozilo
        public async Task<IActionResult> Index()
        {
            var eParkingContext = _context.Vozilo.Include(v => v.Korisnik);
            return View(await eParkingContext.ToListAsync());
        }

        // GET: Vozilo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vozilo = await _context.Vozilo
                .Include(v => v.Korisnik)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vozilo == null)
            {
                return NotFound();
            }

            return View(vozilo);
        }

        // GET: Vozilo/Create
        public IActionResult Create()
        {
            //ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "ID", "ImePrezime");
            return View();
        }

        // POST: Vozilo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vozilo vozilo)
        {
            if (ModelState.IsValid)
            {
                Clan data = Newtonsoft.Json.JsonConvert.DeserializeObject<Clan>((string)TempData["mydata"]);
                vozilo.KorisnikId = data.ID;
                
                _context.Add(vozilo);

                await _context.SaveChangesAsync();
                TempData["vozilo"] = Newtonsoft.Json.JsonConvert.SerializeObject(vozilo);
                List<ParkingLokacija> ParkingLokacije = _context.ParkingLokacija.ToList();
                Vlasnik vlasnik = new Vlasnik();
                foreach (var p in ParkingLokacije)
                {
                    if(p.ID == data.RezervisanoParkingMjesto)
                    {
                        vlasnik.ID = p.VlasnikId;
                    }
                }
                TempData["vlasnik"] = Newtonsoft.Json.JsonConvert.SerializeObject(vlasnik);
                return RedirectToAction("Create", "Zahtjev");
                //return RedirectToAction(nameof(Index));
            }
            //ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "ID", "ImePrezime", vozilo.KorisnikId);
            return View(vozilo);
        }

        // GET: Vozilo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vozilo = await _context.Vozilo.FindAsync(id);
            if (vozilo == null)
            {
                return NotFound();
            }
            //ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "ID", "ImePrezime", vozilo.KorisnikId);
            return View(vozilo);
        }

        // POST: Vozilo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModelAuta,BrojTablice,BrojSasije,BrojMotora,KorisnikId")] Vozilo vozilo)
        {
            if (id != vozilo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vozilo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoziloExists(vozilo.ID))
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
            //ViewData["KorisnikId"] = new SelectList(_context.Korisnik, "ID", "ImePrezime", vozilo.KorisnikId);
            return View(vozilo);
        }

        // GET: Vozilo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vozilo = await _context.Vozilo
                .Include(v => v.Korisnik)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vozilo == null)
            {
                return NotFound();
            }

            return View(vozilo);
        }

        // POST: Vozilo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vozilo = await _context.Vozilo.FindAsync(id);
            _context.Vozilo.Remove(vozilo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoziloExists(int id)
        {
            return _context.Vozilo.Any(e => e.ID == id);
        }
    }
}
