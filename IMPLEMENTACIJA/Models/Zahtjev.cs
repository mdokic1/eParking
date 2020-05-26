using System.ComponentModel.DataAnnotations;

namespace EParking.Models
{
    public class Zahtjev
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        //[Required]
        //public virtual Korisnik Korisnik { get; set; }
        [Required]
        public virtual Vozilo Vozilo { get; set; }
        [Required]
        public virtual Vlasnik Vlasnik { get; set; }
    }
}