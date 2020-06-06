using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EParking.Models
{
    public class EParkingFacade
    {
        private static readonly EParkingFacade INSTANCE = new EParkingFacade();
        public static Administrator Administrator { get; set; }
        public static Clan Clan { get; set; }
        public static Vlasnik Vlasnik { get; set; }
        public List<Transakcija> HistorijaTransakcija { get; set; }
        public List<ParkingLokacija> Parkinzi { get; set; }

        private EParkingFacade() 
        {
            HistorijaTransakcija = new List<Transakcija>();
            Administrator = null;
            Clan = null;
            Vlasnik = null;
        }
        public static EParkingFacade Instance
        {
            get
            {
                return INSTANCE;
            }
        }
        public static bool ClanSignedIn()
        {
            return EParkingFacade.Clan != null;
        }
        public static bool VlasnikSignedIn()
        {
            return EParkingFacade.Vlasnik != null;
        }
        public static bool AdminSignedIn()
        {
            return EParkingFacade.Administrator != null;
        }
    }
}
