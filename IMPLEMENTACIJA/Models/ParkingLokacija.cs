using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EParking.Models
{
    public class ParkingLokacija
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [Required]
        public string Naziv { get; set; }
        [Required]
        public string Adresa { get; set; }
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Long { get; set; }
        [Required]
        public int Kapacitet { get; set; }
        [Required]
        [Display(Name = "Broj slobodnih mjesta")]
        public int BrojSlobodnihMjesta { get; set; }
        
        [Display(Name = "Cjenovnik")]
        public int CjenovnikId { get; set; }
        public virtual Cjenovnik Cjenovnik { get; set; }

        [Display(Name = "Vlasnik")]
        public int VlasnikId { get; set; }
        public virtual Vlasnik Vlasnik { get; set; }
    }
}