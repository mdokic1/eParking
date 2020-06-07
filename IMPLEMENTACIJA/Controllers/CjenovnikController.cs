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
    public class CjenovnikController : Controller
    {
        private readonly EParkingContext _context;

        public CjenovnikController(EParkingContext context)
        {
            _context = context;
        }

        // GET: Cjenovnik
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cjenovnik.ToListAsync());
        }

        // GET: Cjenovnik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //id -= 1;
            var cjenovnik = await _context.Cjenovnik
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cjenovnik == null)
            {
                return NotFound();
            }

            return View(cjenovnik);
        }

        // GET: Cjenovnik/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cjenovnik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,DnevnaCijenaSat,NocnaCijenaSat,CijenaMjesecneKarte,CijenaGodisnjeKarte,Popust")] Cjenovnik cjenovnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cjenovnik);
                await _context.SaveChangesAsync();
                TempData["cjenovnik"] = Newtonsoft.Json.JsonConvert.SerializeObject(cjenovnik);
                return RedirectToAction("Create", "ParkingLokacija");
            }
            return View(cjenovnik);
        }

        // GET: Cjenovnik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //id -= 1;
            var cjenovnik = await _context.Cjenovnik.FindAsync(id);
            if (cjenovnik == null)
            {
                return NotFound();
            }
            return View(cjenovnik);
        }

        // POST: Cjenovnik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Naziv,DnevnaCijenaSat,NocnaCijenaSat,CijenaMjesecneKarte,CijenaGodisnjeKarte,Popust")] Cjenovnik cjenovnik)
        {
            //dodano jer nije radio edit
            cjenovnik.ID = id;
            if (id != cjenovnik.ID)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cjenovnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CjenovnikExists(cjenovnik.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                List<ParkingLokacija> parkinzi = _context.ParkingLokacija.ToList();
                foreach(var p in parkinzi)
                {
                    if(p.CjenovnikId == cjenovnik.ID)
                    {
                        return RedirectToAction("Edit", "ParkingLokacija", new { id = p.ID });
                    }
                }
                
            }
            return View(cjenovnik);
        }

        // GET: Cjenovnik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //id -= 1;
            var cjenovnik = await _context.Cjenovnik
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cjenovnik == null)
            {
                return NotFound();
            }

            return View(cjenovnik);
        }

        // POST: Cjenovnik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //id -= 1;
            var cjenovnik = await _context.Cjenovnik.FindAsync(id);
            _context.Cjenovnik.Remove(cjenovnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CjenovnikExists(int id)
        {
            //id -= 1;
            return _context.Cjenovnik.Any(e => e.ID == id);
        }
    }
}
