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
        public string ModelAuta { get; set; }
        [Required]
        public string BrojTablice { get; set; }
        [Required]
        public string BrojSasije { get; set; }
        [Required]
        public string BrojMotora { get; set; }
        [Required]
        public virtual Korisnik Korisnik { get; set; }
    }
}
