using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EParking.Models
{
    public class EParkingFacade
    {
        private static readonly EParkingFacade INSTANCE = new EParkingFacade();
        public List<Gost> Gosti { get; set; }
        public List<Clan> Clanovi { get; set; }
        public List<Vlasnik> Vlasnici { get; set; }
        public List<Transakcija> HistorijaTransakcija { get; set; }
        public Administrator Administrator { get; set; }
        public List<ParkingLokacija> Parkinzi { get; set; }

        private EParkingFacade() 
        {
            HistorijaTransakcija = new List<Transakcija>();
        }
        public static EParkingFacade Instance
        {
            get
            {
                return INSTANCE;
            }
        }
    }
}
