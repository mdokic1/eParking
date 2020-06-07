
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EParking.Models;
using System.ComponentModel.Design;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.Formatters;

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

        public async Task<IActionResult> OdobravanjeZahtjeva(int? id)
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
             
            MailMessage mail = new MailMessage();

            List<Clan> clanovi = _context.Clan.ToList();
            Clan clan = null;
            foreach(var c in clanovi)
            {
                if(zahtjev.Vozilo.KorisnikId == c.ID)
                {
                    clan = c;
                    c.StatusClanarine = StatusClanarine.ACTIVE;
                    _context.Clan.Update(c);
                    _context.SaveChanges();
                    mail.To.Add(c.Email);
                }
            }

            List<Vozilo> vozila = _context.Vozilo.ToList();
            foreach(var v in vozila)
            {
                if(zahtjev.VoziloId == v.ID)
                {
                    v.DatumRegistracije = DateTime.Now;
                    _context.Vozilo.Update(v);
                    _context.SaveChanges();
                }
            }
            Zahtjev temp = zahtjev;

           // _context.Zahtjev.Remove(zahtjev);
            //await _context.SaveChangesAsync();

            List<Vlasnik> vlasnici = _context.Vlasnik.ToList();
            Vlasnik vlasnik = null;
            foreach(var v in vlasnici)
            {
                if(temp.VlasnikId == v.ID)
                {
                    vlasnik = v;
                    
                    mail.From = new MailAddress("eparking2020@gmail.com");
                    //return RedirectToAction("Account", "Vlasnik", v);
                    //return RedirectToAction("EMail");
                    //return View(zahtjev);
                }
            }

            mail.Subject = "Obrada zahtjeva";
            mail.Body = "Poštovani,\n " + 
                "        želimo da Vas obavijestimo da je Vaš zahtjev za članarinu odobren. \n" +
                "        Uplatu članarine trebate izvršiti na račun 11111111111111111 u roku od 5 radnih dana, " +
                "        nakon čega će status Vaše članarine postati  aktivan.\n" +
                "        " + "\n" +
                "        Srdačan pozdrav, \n"
                      + vlasnik.ImePrezime;
            //mail.Body = mail.Body.Replace("@", System.Environment.NewLine);
            mail.IsBodyHtml = true;

            ViewBag.From = mail.From;
            ViewBag.To = mail.To;
            ViewBag.Subject = mail.Subject;
            ViewBag.Body = mail.Body;

            //TempData["mail"] = Newtonsoft.Json.JsonConvert.SerializeObject(mail);
            //MailMessage email = Newtonsoft.Json.JsonConvert.DeserializeObject<MailMessage>((string)TempData["mail"]);
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential("eparking2020", "grupa5eparking");
            smtp.EnableSsl = true;
            smtp.Send(mail);

            return View(temp);
        }

        
        public async Task<IActionResult> EMail(int? id)
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

            Zahtjev temp = zahtjev;

            _context.Zahtjev.Remove(zahtjev);
            await _context.SaveChangesAsync();

            Vlasnik vlasnik = null;

            List<Vlasnik> vlasnici = _context.Vlasnik.ToList();
            foreach (var v in vlasnici)
            {
                if (temp.VlasnikId == v.ID)
                {
                    vlasnik = v;
                    //mail.From = new MailAddress("eparking2020@gmail.com");
                    //return RedirectToAction("Account", "Vlasnik", v);
                    //return RedirectToAction("EMail");
                    //return View(zahtjev);
                }
            }

            

            ViewBag.Vlasnik = vlasnik;
            return RedirectToAction("Account", "Vlasnik", vlasnik);
            
        }
       

        public async Task<IActionResult> OdbijanjeZahtjeva(int? id)
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

            Zahtjev temp = zahtjev;

            _context.Zahtjev.Remove(zahtjev);
            await _context.SaveChangesAsync();

            List<Vlasnik> vlasnici = _context.Vlasnik.ToList();
            foreach (var v in vlasnici)
            {
                if (temp.VlasnikId == v.ID)
                {
                    ViewBag.Vlasnik = v;
                    return RedirectToAction("Account", "Vlasnik", v);
                    //return View(zahtjev);
                }
            }

            return View(temp);
        }

        public async Task<IActionResult> Obrada(int? id)
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

            List<Clan> clanovi = _context.Clan.ToList();
            List<ParkingLokacija> parkinzi = _context.ParkingLokacija.ToList();
            foreach(var c in clanovi)
            {
                if(zahtjev.Vozilo.KorisnikId == c.ID)
                {
                    ViewBag.Korisnik = c;
                    foreach(var p in parkinzi)
                    {
                        if(c.RezervisanoParkingMjesto == p.ID)
                        {
                            ViewBag.Parking = p;
                        }
                    }
                    return View(zahtjev);
                }
            }

            return View(zahtjev);
        }

        // GET: Zahtjev/Create
        public IActionResult Create()
        {
            //ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime");
            //ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora");
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
                Vozilo voziloData = Newtonsoft.Json.JsonConvert.DeserializeObject<Vozilo>((string)TempData["vozilo"]);
                Vlasnik vlasnikData = Newtonsoft.Json.JsonConvert.DeserializeObject<Vlasnik>((string)TempData["vlasnik"]);

                zahtjev.VlasnikId = vlasnikData.ID;
                zahtjev.VoziloId = voziloData.ID;
                _context.Add(zahtjev);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            //ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime", zahtjev.VlasnikId);
            //ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora", zahtjev.VoziloId);
            //OVO KORISTIM KAKO BIH PRIKAZAO ALERT
            ViewBag.Alert = "'show'";
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
           // ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime", zahtjev.VlasnikId);
            //ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora", zahtjev.VoziloId);
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
            //ViewData["VlasnikId"] = new SelectList(_context.Vlasnik, "ID", "ImePrezime", zahtjev.VlasnikId);
           // ViewData["VoziloId"] = new SelectList(_context.Vozilo, "ID", "BrojMotora", zahtjev.VoziloId);
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
