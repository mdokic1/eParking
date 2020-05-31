using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EParking.Models;
using Microsoft.AspNetCore.Mvc;

namespace EParking.Controllers
{
    public class EParkingController : Controller
    {
        private readonly EParkingContext _context;
        ParkingLokacija odredisnaParkingLokacija = new ParkingLokacija();

        public EParkingController(EParkingContext context)
        {
            _context = context;
        }
        public IActionResult Map()
        {
            EParkingFacade.Instance.Parkinzi = _context.ParkingLokacija.ToList();
            List<Cjenovnik> Cjenovnici = _context.Cjenovnik.ToList();
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
            string markeri = "[";
            int vel = 0;
            foreach (ParkingLokacija parking in EParkingFacade.Instance.Parkinzi)
            {
                markeri += "{";
                double lat = parking.Lat;
                double lng = parking.Long;
                markeri += String.Format("'lat': '{0}',", lat.ToString(System.Globalization.CultureInfo.InvariantCulture));
                markeri += String.Format("'lng': '{0}',", lng.ToString(System.Globalization.CultureInfo.InvariantCulture));
                //cijena po satu nije kako treba spasena u parking lokaciji u bazi pa zato ovo
                //ISPRAVI TO KASNIJE!!!
                markeri += string.Format("'naziv': '{0}',", parking.Naziv);
                markeri += string.Format("'adresa': '{0}',", parking.Adresa);
                markeri += string.Format("'cijena': '{0}',", parking.Cjenovnik.DnevnaCijenaSat);
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
            //JOS FALI DA NAMJESTIS UKOLIKO JE KORISNIK KOJI KLIKA CLAN DA PRIKAZE NJEGOV PARKING
            //A UKOLIKO JE GOST DA PRIKAZE NAJBLIZI PARKING OD NJEGOVE TRENUTNE LOKACIJE
            //true -- najblizi parking
            //false -- rezrvisani parking
            ViewBag.NajbliziParking = "'true';";
            ViewBag.Latitude = EParkingFacade.Instance.Parkinzi.ElementAt(2).Lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
            ViewBag.Longitude = EParkingFacade.Instance.Parkinzi.ElementAt(2).Long.ToString(System.Globalization.CultureInfo.InvariantCulture);
            return View(EParkingFacade.Instance);
        }
        [HttpPost]
        public void SpasiKoordinate(double lat, double lon)
        {
            foreach (var p in EParkingFacade.Instance.Parkinzi)
            {
                if (p.Lat == lat && p.Long == lon)
                {
                    odredisnaParkingLokacija = p;
                    break;
                }
            }
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
                    return RedirectToAction("Account", "Vlasnik");
                }
            }

            foreach (var a in administratori)
            {
                if (a.Username == username && a.Password == password)
                {
                    pronadjeno = true;
                    return RedirectToAction("Account", "Administrator");
                }
            }
            return View();
        }
    }
}