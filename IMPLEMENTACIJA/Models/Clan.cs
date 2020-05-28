using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EParking.Models
{
    public class Clan: Korisnik
    {
        [Required]
        [Display(Name = "Choose parking spot")]
        public int RezervisanoParkingMjesto { get; set; }
        [Required]
        [Display(Name = "Status")]
        public StatusClanarine StatusClanarine { get; set; }
        [Required]
        [Display(Name = "Type of membership")]
        public TipClanarine TipClanarine { get; set; }
    }
}
