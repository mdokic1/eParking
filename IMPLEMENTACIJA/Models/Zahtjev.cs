using System.ComponentModel.DataAnnotations;

namespace EParking.Models
{
    public class Zahtjev
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        //[Required]
        //public virtual Korisnik Korisnik { get; set; }

        [Display(Name = "Registarske tablice")]
        public int VoziloId { get; set; }
        
        public virtual Vozilo Vozilo { get; set; }

        [Display(Name = "Ime vlasnika parkinga")]
        public int VlasnikId { get; set; }
        
        public virtual Vlasnik Vlasnik { get; set; }
    }
}