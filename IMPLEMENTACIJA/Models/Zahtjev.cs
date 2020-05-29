using System.ComponentModel.DataAnnotations;

namespace EParking.Models
{
    public class Zahtjev
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        //[Required]
        //public virtual Korisnik Korisnik { get; set; }
        
        public int VoziloId { get; set; }
        
        public virtual Vozilo Vozilo { get; set; }

        
        public int VlasnikId { get; set; }
        
        public virtual Vlasnik Vlasnik { get; set; }
    }
}