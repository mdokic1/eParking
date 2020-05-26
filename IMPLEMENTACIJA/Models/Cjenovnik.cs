using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EParking.Models
{
    public class Cjenovnik
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [Required]
        public string Naziv { get; set; }
        [Required]
        public double DnevnaCijenaSat { get; set; }
        [Required]
        public double NocnaCijenaSat { get; set; }
        [Required]
        public double CijenaMjesecneKarte { get; set; }
        [Required]
        public double CijenaGodisnjeKarte { get; set; }
        [Required]
        public double Popust { get; set; }
        public virtual ICollection<ParkingLokacija> ParkingLokacije { get; set; }
    }
}