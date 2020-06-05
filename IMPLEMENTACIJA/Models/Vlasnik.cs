using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EParking.Models
{
    public class Vlasnik
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Ime i prezime")]
        public string ImePrezime { get; set; }
        [Required]
        public double Prihodi { get; set; }
        public virtual ICollection<Zahtjev> Zahtjevi { get; set; }
        
      
    }
}
