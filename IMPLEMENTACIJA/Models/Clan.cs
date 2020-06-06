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
        [Display(Name = "Izaberite parking mjesto")]
        public int RezervisanoParkingMjesto { get; set; }
        [Required]
        [Display(Name = "Status")]
        public StatusClanarine StatusClanarine { get; set; }
        [Required]
        [Display(Name = "Tip članarine (MJESECNA/ GODISNJA)")]
        public TipClanarine TipClanarine { get; set; }
    }
}
