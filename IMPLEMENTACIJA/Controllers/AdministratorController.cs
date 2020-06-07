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
    public class AdministratorController : Controller
    {
        private readonly EParkingContext _context;

        public AdministratorController(EParkingContext context)
        {
            _context = context;
        }

        // GET: Administrator
        public IActionResult Index(string username, string password)
        {
            List<Administrator> administratori = _context.Administrator.ToList();
            foreach(var a in administratori)
            {
                if(a.Username == username && a.Password == password)
                {
                    return View(a);
                }
            }
            return View();
        }

        public IActionResult Account(string username, string password)
        {

            if (username == null && password == null)
            {
                username = EParkingFacade.Administrator.Username;
                password = EParkingFacade.Administrator.Password;
            }
            List<Vlasnik> vlasnici = _context.Vlasnik.ToList();
            List<ParkingLokacija> parkinzi = _context.ParkingLokacija.ToList();
            List<Clan> clanovi = _context.Clan.ToList();
            int brojVlasnika = 0;
            int brojParkinga = 0;
            int brojClanova = 0;
            foreach(var v in vlasnici)
            {
                brojVlasnika++;
            }

            foreach(var p in parkinzi)
            {
                brojParkinga++;
            }

            foreach(var c in clanovi)
            {
                brojClanova++;
            }

            List<Administrator> administratori = _context.Administrator.ToList();
            foreach(var a in administratori)
            {
                if(a.Username == username && a.Password == password)
                {
                    ViewBag.Vlasnici = brojVlasnika;
                    ViewBag.Parkinzi = brojParkinga;
                    ViewBag.Clanovi = brojClanova;
                    return View(a);
                }
            }
            return View();
            
        }

        public IActionResult Logout()
        {
            EParkingFacade.Administrator = null;
            return RedirectToAction("Login", "Eparking");
        }

        // GET: Administrator/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrator
                .FirstOrDefaultAsync(m => m.ID == id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // GET: Administrator/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrator/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Password")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administrator);
                await _context.SaveChangesAsync();
               // return RedirectToAction(nameof(Index));
            }
            return View(administrator);
        }

        // GET: Administrator/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrator.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }
            return View(administrator);
        }

        // POST: Administrator/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Username,Password")] Administrator administrator)
        {
            administrator.ID = id;
            if (id != administrator.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administrator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministratorExists(administrator.ID))
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
            return View(administrator);
        }

        // GET: Administrator/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrator
                .FirstOrDefaultAsync(m => m.ID == id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // POST: Administrator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var administrator = await _context.Administrator.FindAsync(id);
            _context.Administrator.Remove(administrator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministratorExists(int id)
        {
            return _context.Administrator.Any(e => e.ID == id);
        }
    }
}
