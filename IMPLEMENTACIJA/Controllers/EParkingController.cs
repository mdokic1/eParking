using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EParking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EParking.Controllers
{
    public class EParkingController : Controller
    {
        private readonly EParkingContext _context;
        private double KonacniIznos { get; set; }

        public EParkingController(EParkingContext context)
        {
            _context = context;
        }

        public double PrilagodiCijenu(double dnevnaCijena, double nocnaCijena)
        {
            TimeSpan nightStart = new TimeSpan(22, 0, 0);
            TimeSpan nightEnd = new TimeSpan(07, 0, 0);
            if (nightStart < nightEnd)
            {
                if (nightStart <= DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay <= nightEnd)
                {
                    return nocnaCijena;
                }
                return dnevnaCijena;
            }
            else if (!(nightEnd < DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < nightStart))
            {
                return nocnaCijena;
            }
            return dnevnaCijena;
        }
        public IActionResult Map()
        {
            EParkingFacade.Instance.Parkinzi = _context.ParkingLokacija.ToList();
            List<Cjenovnik> Cjenovnici = _context.Cjenovnik.ToList();
            List<Vlasnik> Vlasnici = _context.Vlasnik.ToList();
            foreach (var p in EParkingFacade.Instance.Parkinzi)
            {
                foreach (var c in Cjenovnici)
                {
                    if (p.CjenovnikId == c.ID)
                    {
                        p.Cjenovnik = c;
                    }
                }
            }
            foreach (var p in EParkingFacade.Instance.Parkinzi)
            {
                foreach (var v in Vlasnici)
                {
                    if (v.ID == p.VlasnikId)
                    {
                        p.Vlasnik = v;
                    }
                }
            }
            string markeri = "[";
            int vel = 0;
            foreach (ParkingLokacija parking in EParkingFacade.Instance.Parkinzi)
            {
                markeri += "{";
                double lat = parking.Lat;
                double lng = parking.Long;
                markeri += String.Format("'lat': '{0}',", lat.ToString(System.Globalization.CultureInfo.InvariantCulture));
                markeri += String.Format("'lng': '{0}',", lng.ToString(System.Globalization.CultureInfo.InvariantCulture));
                markeri += string.Format("'naziv': '{0}',", parking.Naziv);
                markeri += string.Format("'adresa': '{0}',", parking.Adresa);
                markeri += string.Format("'cijena': '{0}',", PrilagodiCijenu(parking.Cjenovnik.DnevnaCijenaSat, parking.Cjenovnik.NocnaCijenaSat));
                markeri += string.Format("'slobodnaMjesta': '{0}',", parking.BrojSlobodnihMjesta);
                markeri += string.Format("'kapacitet': '{0}'", parking.Kapacitet);
                if (vel < EParkingFacade.Instance.Parkinzi.Count - 1)
                {
                    markeri += "},";
                }
                else
                {
                    markeri += "}";
                }
                vel++;
            }
            markeri += "];";
            ViewBag.Markeri = markeri;
            if (EParkingFacade.ClanSignedIn())
            {
                ViewBag.NajbliziParking = "'false';";
                foreach (var p in EParkingFacade.Instance.Parkinzi)
                {
                    if (p.ID == EParkingFacade.Clan.RezervisanoParkingMjesto)
                    {
                        ViewBag.Latitude = p.Lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        ViewBag.Longitude = p.Long.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }
            else
            {
                ViewBag.NajbliziParking = "'true';";
                ViewBag.Latitude = EParkingFacade.Instance.Parkinzi.ElementAt(0).Lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
                ViewBag.Longitude = EParkingFacade.Instance.Parkinzi.ElementAt(0).Long.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            return View(EParkingFacade.Instance);
        }

        [HttpPost]
        public async Task<IActionResult> Map(double lat, double lon)
        {
            ParkingLokacija odredisnaParkingLokacija = null;
            foreach (var p in EParkingFacade.Instance.Parkinzi)
            {
                if (p.Lat == lat && p.Long == lon)
                {
                    odredisnaParkingLokacija = p;
                    break;
                }
            }

            //azuriranje baze smanji se broj slobodnih mjesta
            odredisnaParkingLokacija.BrojSlobodnihMjesta -= 1;
            _context.Update(odredisnaParkingLokacija);
            await _context.SaveChangesAsync();
            //-----------------------------------------------

            //otvaramo Timer view odmah
            TempData["cijena"] = Newtonsoft.Json.JsonConvert.SerializeObject(PrilagodiCijenu(odredisnaParkingLokacija.Cjenovnik.DnevnaCijenaSat, odredisnaParkingLokacija.Cjenovnik.NocnaCijenaSat));
            return RedirectToAction("Timer", "EParking", odredisnaParkingLokacija);
        }

        
        public IActionResult Timer(ParkingLokacija odredisnaParkingLokacija = null)
        {
            List<Cjenovnik> Cjenovnici = _context.Cjenovnik.ToList();
            foreach (var c in Cjenovnici)
            {
                if (odredisnaParkingLokacija.CjenovnikId == c.ID)
                {
                    odredisnaParkingLokacija.Cjenovnik = c;
                }
            }
            ViewBag.Cijena = Newtonsoft.Json.JsonConvert.DeserializeObject<double>((string)TempData["cijena"]);
            Transakcija novaTransakcija = new Transakcija();
            novaTransakcija.VrijemeDolaska = DateTime.Now;
            novaTransakcija.ParkingLokacijaId = odredisnaParkingLokacija.ID;
            EParkingFacade.Instance.HistorijaTransakcija.Add(novaTransakcija);
            return View(odredisnaParkingLokacija);
        }
        
        [HttpPost]
        public async Task<IActionResult> TimerAsync(double iznos, ParkingLokacija parkingLokacija)
        {
            //pronalazak odgovarajućeg vozila
            //postavljamo default vrijednost vozila ako se radi o Gostu
            Vozilo vozilo = _context.Vozilo.ToList().ElementAt(0);
            foreach (var v in _context.Vozilo.ToList())
            {
                if (EParkingFacade.ClanSignedIn() && v.KorisnikId == EParkingFacade.Clan.ID)
                {
                    vozilo = v;
                    break;
                }
            }
            //-------------------------------

            //dodaj transakciju
            foreach (var t in EParkingFacade.Instance.HistorijaTransakcija)
            {
                if (t.ParkingLokacijaId == parkingLokacija.ID)
                {
                    t.VrijemeOdlaska = DateTime.Now;
                    t.Iznos = iznos;
                    t.VoziloId = vozilo.ID;
                    _context.Transakcija.Add(t);
                    await _context.SaveChangesAsync();
                }
            }
            //--------------------

            //oslobodi zauzeto mjesto na parkingu
            foreach (var p in EParkingFacade.Instance.Parkinzi)
            {
                if (p.ID == parkingLokacija.ID)
                {
                    parkingLokacija = p;
                    break;
                }
            }
            parkingLokacija.BrojSlobodnihMjesta += 1;
            _context.Update(parkingLokacija);
            await _context.SaveChangesAsync();
            //-----------------------------------

            TempData["iznos"] = Newtonsoft.Json.JsonConvert.SerializeObject(iznos);
            TempData["parkingLokacijaID"] = Newtonsoft.Json.JsonConvert.SerializeObject(parkingLokacija.ID);
            return RedirectToAction("Pay", "EParking");
        }

        [HttpPost]
        public async Task<IActionResult> PayAsync(string username, double cardNumber, int month, int year, int cvv)
        {
            ViewBag.Alert = "'show'";
            //dodajemo Vlasniku parkinga prihode
            double prihodi = Newtonsoft.Json.JsonConvert.DeserializeObject<double>((string)TempData["prihodi"]);
            int parkingLokacijaID = Newtonsoft.Json.JsonConvert.DeserializeObject<int>((string)TempData["parkingLokacijaID"]);
            List<Vlasnik> vlasnici = _context.Vlasnik.ToList();
            ParkingLokacija vlasnikovParking = new ParkingLokacija();
            foreach (var p in EParkingFacade.Instance.Parkinzi)
            {
                if (p.ID == parkingLokacijaID)
                {
                    vlasnikovParking = p;
                    break;
                }
            }
            foreach (var v in vlasnici)
            {
                if (vlasnikovParking.VlasnikId == v.ID)
                {
                    v.Prihodi += prihodi;
                    _context.Update(v);
                    await _context.SaveChangesAsync();
                    break;
                }
            }
            //----------------------------------
            return View();
        }

        public IActionResult Pay()
        {            
            ViewBag.Iznos = Newtonsoft.Json.JsonConvert.DeserializeObject<double>((string)TempData["iznos"]); 
            TempData["prihodi"] = Newtonsoft.Json.JsonConvert.SerializeObject(ViewBag.Iznos);
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            List<Korisnik> korisnici = _context.Korisnik.ToList();
            List<Vlasnik> vlasnici = _context.Vlasnik.ToList();
            List<Administrator> administratori = _context.Administrator.ToList();
            List<Vozilo> vozila = _context.Vozilo.ToList();
            bool pronadjeno = false;
            bool postoji = false;
            
            foreach(var k in korisnici)
            {
                if(k.Username == username && k.Password == password)
                {
                    pronadjeno = true;
                    foreach(var v in vozila)
                    {
                        if(v.KorisnikId == k.ID)
                        {
                            //TempData["v"] = Newtonsoft.Json.JsonConvert.SerializeObject(v);
                            EParkingFacade.Clan = (Clan)k;
                            return RedirectToAction("Account", "Clan", k);
                        }
                    }
                   
                }
            }

            foreach (var v in vlasnici)
            {
                if (v.Username == username && v.Password == password)
                {
                    pronadjeno = true;
                    EParkingFacade.Vlasnik = v;
                    return RedirectToAction("Account", "Vlasnik", v);
                }
            }

            foreach (var a in administratori)
            {
                if (a.Username == username && a.Password == password)
                {
                    pronadjeno = true;
                    EParkingFacade.Administrator = a;
                    return RedirectToAction("Account", "Administrator", a);
                }
            }
            return View();
        }
    }
}