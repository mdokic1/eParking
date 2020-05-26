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
        public int RezervisanoParkingMjesto { get; set; }
        [Required]
        public StatusClanarine StatusClanarine { get; set; }
        [Required]
        public TipClanarine TipClanarine { get; set; }
    }
}
