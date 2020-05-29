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
                //return RedirectToAction(nameof(Index));
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
            return View(clan);
        }

        // POST: Clan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RezervisanoParkingMjesto,StatusClanarine,TipClanarine,ImePrezime,Username,Password,JMBG,Adresa,BrojMobitela,Email")] Clan clan)
        {
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
                return RedirectToAction(nameof(Index));
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
