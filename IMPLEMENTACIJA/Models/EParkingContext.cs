using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EParking.Models
{
    public class EParkingContext: DbContext
    {
        public EParkingContext(DbContextOptions<EParkingContext> options) : base(options)
        {
        }

        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Vlasnik> Vlasnik { get; set; }
        public DbSet<ParkingLokacija> ParkingLokacija { get; set; }
        public DbSet<Transakcija> Transakcija { get; set; }
        public DbSet<Cjenovnik> Cjenovnik { get; set; }
        public DbSet<Vozilo> Vozilo { get; set; }
        public DbSet<Zahtjev> Zahtjev { get; set; }
        public DbSet<Clan> Clan { get; set; }
        public DbSet<Gost> Gost { get; set; }

        public DbSet<Korisnik> Korisnik { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>().ToTable("Administrator");
            modelBuilder.Entity<Vlasnik>().ToTable("Vlasnik");
            modelBuilder.Entity<ParkingLokacija>().ToTable("ParkingLokacija");
            modelBuilder.Entity<Transakcija>().ToTable("Transakcija");
            modelBuilder.Entity<Cjenovnik>().ToTable("Cjenovnik");
            modelBuilder.Entity<Vozilo>().ToTable("Vozilo");
            modelBuilder.Entity<Zahtjev>().ToTable("Zahtjev");
            modelBuilder.Entity<Korisnik>().ToTable("Korisnik");
        }

    }
}
