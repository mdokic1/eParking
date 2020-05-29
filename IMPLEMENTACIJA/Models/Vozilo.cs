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
        [Display(Name = "Car model")]
        public string ModelAuta { get; set; }
        [Required]
        [Display(Name = "Licence plate")]
        public string BrojTablice { get; set; }
        [Required]
        [Display(Name = "Chassis number")]
        public string BrojSasije { get; set; }
        [Required]
                
        [Display(Name = "Engine number")]
        public string BrojMotora { get; set; }
        [Required]
        [Display(Name = "Name")]
        //[Compare(Korisnik.ImePrezime)]
        public int KorisnikId { get; set; }
        public virtual Korisnik Korisnik { get; set; }
    }
}
