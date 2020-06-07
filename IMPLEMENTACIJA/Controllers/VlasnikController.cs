using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EParking.Models;
using Newtonsoft.Json;
using EParkingOOAD.ViewModel;

namespace EParkingOOAD.Controllers
{
    public class VlasnikController : Controller
    {
        private readonly EParkingContext _context;

        public VlasnikController(EParkingContext context)
        {
            _context = context;
        }

        // GET: Vlasnik
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vlasnik.ToListAsync());
        }

        public bool IdeUDanGraf(DateTime date)
        {
            return date.Date == DateTime.Today;
        }

        public bool IdeUMjesecGraf(DateTime date)
        {
            return DateTime.Now.AddDays(-30).CompareTo(date) < 0;
        }

        public void NapuniGrafovePodacima(Vlasnik v)
        {
            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            List<Tacka> VrijednostiDan = new List<Tacka>();
            List<Tacka> VrijednostiMjesec = new List<Tacka>();
            int i = 1, j = 1, parkingLokacijaID = 0;
            foreach (var p in _context.ParkingLokacija.ToList())
            {
                if (p.VlasnikId == v.ID)
                {
                    parkingLokacijaID = p.ID;
                    foreach (var t in _context.Transakcija.ToList())
                    {
                        if (t.ParkingLokacijaId == parkingLokacijaID)
                        {
                            if (IdeUDanGraf(t.VrijemeDolaska))
                            {
                                VrijednostiDan.Add(new Tacka(i, t.Iznos));
                                i++;
                            }
                            if (IdeUMjesecGraf(t.VrijemeDolaska))
                            {
                                VrijednostiMjesec.Add(new Tacka(j, t.Iznos));
                                j++;
                            }
                        }
                    }
                }
            }
            ViewBag.DataPointsDan = JsonConvert.SerializeObject(VrijednostiDan, _jsonSetting);
            ViewBag.DataPointsMjesec = JsonConvert.SerializeObject(VrijednostiMjesec, _jsonSetting);
        }

        public IActionResult Account(string username, string password)
        {
            if(username == null && password == null)
            {
                username = EParkingFacade.Vlasnik.Username;
                password = EParkingFacade.Vlasnik.Password;
            }
            List<Vlasnik> vlasnici = _context.Vlasnik.ToList();
            List<Zahtjev> odgovarajuci = new List<Zahtjev>();
            foreach (var v in vlasnici)
            {
                if (v.Username == username && v.Password == password)
                {
                    List<Zahtjev> zahtjevi = _context.Zahtjev.ToList();
                    foreach(var z in zahtjevi)
                    {
                        if(z.VlasnikId == v.ID)
                        {
                            odgovarajuci.Add(z);
                        }
                    }
                    ViewBag.zahtjevi = odgovarajuci;
                    NapuniGrafovePodacima(v);
                    return View(v);
                }
            }
            return View();
        }

        
        public async Task<IActionResult> Izmjena(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vlasnik = await _context.Vlasnik
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vlasnik == null)
            {
                return NotFound();
            }

            List<ParkingLokacija> parkinzi = _context.ParkingLokacija.ToList();
            List<Cjenovnik> cjenovnici = _context.Cjenovnik.ToList();
            foreach(var p in parkinzi)
            {
                if(p.VlasnikId == vlasnik.ID)
                {
                    foreach(var c in cjenovnici)
                    {
                        if(p.CjenovnikId == c.ID)
                        {
                            return RedirectToAction("Edit", "Cjenovnik", new { id = c.ID });
                        }
                    }
                }
            }
            return View(vlasnik);
        }

        public IActionResult Logout()
        {
            EParkingFacade.Vlasnik = null;
            return RedirectToAction("Login", "Eparking");
        }

        // GET: Vlasnik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vlasnik = await _context.Vlasnik
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vlasnik == null)
            {
                return NotFound();
            }

            return View(vlasnik);
        }

        // GET: Vlasnik/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vlasnik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vlasnik vlasnik)
        {
            if (ModelState.IsValid)
            {
                vlasnik.Prihodi = 0.0;
                _context.Add(vlasnik);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vlasnik);
        }

        // GET: Vlasnik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vlasnik = await _context.Vlasnik.FindAsync(id);
            if (vlasnik == null)
            {
                return NotFound();
            }
            return View(vlasnik);
        }

        // POST: Vlasnik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Username,Password,ImePrezime,Prihodi")] Vlasnik vlasnik)
        {
            vlasnik.ID = id;
            if (id != vlasnik.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vlasnik);
                    await _context.SaveChangesAsync();
                    EParkingFacade.Vlasnik = vlasnik;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VlasnikExists(vlasnik.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Account", "Vlasnik", EParkingFacade.Vlasnik);
            }
            return View(vlasnik);
        }

        // GET: Vlasnik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vlasnik = await _context.Vlasnik
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vlasnik == null)
            {
                return NotFound();
            }

            return View(vlasnik);
        }

        // POST: Vlasnik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vlasnik = await _context.Vlasnik.FindAsync(id);
            _context.Vlasnik.Remove(vlasnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VlasnikExists(int id)
        {
            return _context.Vlasnik.Any(e => e.ID == id);
        }
    }
}
