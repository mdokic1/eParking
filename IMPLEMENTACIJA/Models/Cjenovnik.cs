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

        [Display(Name = "Dnevna cijena po satu")]
        public double DnevnaCijenaSat { get; set; }
        [Required]

        [Display(Name = "Noćna cijena po satu")]
        public double NocnaCijenaSat { get; set; }
        [Required]

        [Display(Name = "Cijena mjesečne karte")]
        public double CijenaMjesecneKarte { get; set; }
        [Required]

        [Display(Name = "Cijena godišnje karte")]
        public double CijenaGodisnjeKarte { get; set; }
        [Required]
        public double Popust { get; set; }
        
    }
}