using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EParking.Models
{
    public class Vozilo
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Model auta")]
        public string ModelAuta { get; set; }
        [Required]
        [Display(Name = "Registarske tablice")]
        public string BrojTablice { get; set; }
        [Required]
        [Display(Name = "Broj šasije")]
        public string BrojSasije { get; set; }

        [Required]

        [Display(Name = "Datum registracije")]
        public DateTime DatumRegistracije { get; set; }
        
        [Required]
                
        [Display(Name = "Broj motora")]
        public string BrojMotora { get; set; }
        [Required]
        [Display(Name = "Ime vlasnika")]
        //[Compare(Korisnik.ImePrezime)]
        public int KorisnikId { get; set; }
        public virtual Korisnik Korisnik { get; set; }
    }
}
