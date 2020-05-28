using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EParking.Models
{
    public class Transakcija
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [Required]
        public DateTime VrijemeDolaska { get; set; }
        [Required]
        public DateTime VrijemeOdlaska { get; set; }
        [Required]
        public double Iznos { get; set; }
       
        public int ParkingLokacijaId { get; set; }
        
        public virtual ParkingLokacija ParkingLokacija { get; set; }
        
        public int VoziloId { get; set; }
        
        public virtual Vozilo Vozilo { get; set; }
    }
}
