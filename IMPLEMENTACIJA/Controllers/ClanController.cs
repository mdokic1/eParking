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
    public class ClanController : Controller
    {
        private readonly EParkingContext _context;

        public ClanController(EParkingContext context)
        {
            _context = context;
        }

        // GET: Clan
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clan.ToListAsync());
        }

        // GET: Clan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clan
                .FirstOrDefaultAsync(m => m.ID == id);
            if (clan == null)
            {
                return NotFound();
            }

            return View(clan);
        }

        public IActionResult Account(string username, string password)
        {
            if (username == null && password == null)
            {
                username = EParkingFacade.Clan.Username;
                password = EParkingFacade.Clan.Password;
            }
            List<Clan> clanovi = _context.Clan.ToList();
            List<Vozilo> vozila = _context.Vozilo.ToList();
            foreach(var k in clanovi)
            {
                if(k.Username == username && k.Password == password)
                {
             
                    foreach(var v in vozila)
                    {
                        
                        if (v.KorisnikId == k.ID)
                        {
                            if (k.StatusClanarine == StatusClanarine.ACTIVE && k.TipClanarine == TipClanarine.MJESECNA)
                            {
                                if(DateTime.Now > v.DatumRegistracije.AddDays(30))
                                {
                                    k.StatusClanarine = StatusClanarine.INACTIVE;
                                    _context.Clan.Update(k);
                                    _context.SaveChanges();
                                }
                            }

                            if(k.StatusClanarine == StatusClanarine.ACTIVE && k.TipClanarine == TipClanarine.GODISNJA)
                            {
                                if (DateTime.Now > v.DatumRegistracije.AddDays(365))
                                {
                                    k.StatusClanarine = StatusClanarine.INACTIVE;
                                    _context.Clan.Update(k);
                                    _context.SaveChanges();
                                }
                            }

                            ViewBag.Model = v.ModelAuta;
                            ViewBag.Tablice = v.BrojTablice;
                            ViewBag.Sasija = v.BrojSasije;
                            ViewBag.Motor = v.BrojMotora;
                            return View(k);
                        }
                    }
                    return View(k);
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            EParkingFacade.Clan = null;
            return RedirectToAction("Login", "Eparking");
        }

        // GET: Clan/Create
        public IActionResult Create()
        {
            ViewData["RezervisanoParkingMjesto"] = new SelectList(_context.ParkingLokacija, "ID", "Naziv");
            return View();
        }

        // POST: Clan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Clan clan)
        {
            if (ModelState.IsValid)
            {
                clan.StatusClanarine = StatusClanarine.INACTIVE;
                _context.Add(clan);
                
                
                await _context.SaveChangesAsync();
                TempData["mydata"] = Newtonsoft.Json.JsonConvert.SerializeObject(clan);
                return RedirectToAction("Create", "Vozilo");

            }
            ViewData["RezervisanoParkingMjesto"] = new SelectList(_context.ParkingLokacija, "ID", "Naziv", clan.RezervisanoParkingMjesto);
                        
            return View(clan);
        }

        // GET: Clan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clan.FindAsync(id);
            if (clan == null)
            {
                return NotFound();
            }
            ViewData["RezervisanoParkingMjesto"] = new SelectList(_context.ParkingLokacija, "ID", "Naziv", clan.RezervisanoParkingMjesto);
            //return RedirectToAction("Clan", "Edit", new { id = clan.ID });
            return View(clan);
        }

        // POST: Clan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RezervisanoParkingMjesto","StatusClanarine","TipClanarine","ImePrezime","Username","Password","JMBG","Adresa","BrojMobitela","Email")] Clan clan)
        {
            clan.ID = id;
            if (id != clan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClanExists(clan.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                List<Vozilo> vozila = _context.Vozilo.ToList();
                foreach(var v in vozila)
                {
                    if(v.KorisnikId == clan.ID)
                    {
                        //TempData["clanID"] = Newtonsoft.Json.JsonConvert.SerializeObject(clan);
                        return RedirectToAction("Edit", "Vozilo", new { id = v.ID });
                    }
                }
                
            }
            ViewData["RezervisanoParkingMjesto"] = new SelectList(_context.ParkingLokacija, "ID", "Naziv", clan.RezervisanoParkingMjesto);
            return View(clan);
        }

        // GET: Clan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clan
                .FirstOrDefaultAsync(m => m.ID == id);
            if (clan == null)
            {
                return NotFound();
            }

            return View(clan);
        }

        // POST: Clan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clan = await _context.Clan.FindAsync(id);
            _context.Clan.Remove(clan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClanExists(int id)
        {
            return _context.Clan.Any(e => e.ID == id);
        }
    }
}
