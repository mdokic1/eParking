using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EParkingOOAD.Migrations
{
    public partial class Izmjena3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cjenovnik",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: false),
                    DnevnaCijenaSat = table.Column<double>(nullable: false),
                    NocnaCijenaSat = table.Column<double>(nullable: false),
                    CijenaMjesecneKarte = table.Column<double>(nullable: false),
                    CijenaGodisnjeKarte = table.Column<double>(nullable: false),
                    Popust = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cjenovnik", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImePrezime = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    JMBG = table.Column<string>(nullable: false),
                    Adresa = table.Column<string>(nullable: false),
                    BrojMobitela = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    RezervisanoParkingMjesto = table.Column<int>(nullable: true),
                    StatusClanarine = table.Column<int>(nullable: true),
                    TipClanarine = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ParkingLokacija",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: false),
                    Adresa = table.Column<string>(nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    Long = table.Column<double>(nullable: false),
                    Kapacitet = table.Column<int>(nullable: false),
                    BrojSlobodnihMjesta = table.Column<int>(nullable: false),
                    CjenovnikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingLokacija", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ParkingLokacija_Cjenovnik_CjenovnikId",
                        column: x => x.CjenovnikId,
                        principalTable: "Cjenovnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vozilo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelAuta = table.Column<string>(nullable: false),
                    BrojTablice = table.Column<string>(nullable: false),
                    BrojSasije = table.Column<string>(nullable: false),
                    BrojMotora = table.Column<string>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vozilo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Vozilo_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vlasnik",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    ImePrezime = table.Column<string>(nullable: false),
                    Prihodi = table.Column<double>(nullable: false),
                    ParkingLokacijaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vlasnik", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Vlasnik_ParkingLokacija_ParkingLokacijaId",
                        column: x => x.ParkingLokacijaId,
                        principalTable: "ParkingLokacija",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transakcija",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VrijemeDolaska = table.Column<DateTime>(nullable: false),
                    VrijemeOdlaska = table.Column<DateTime>(nullable: false),
                    Iznos = table.Column<double>(nullable: false),
                    ParkingLokacijaId = table.Column<int>(nullable: false),
                    VoziloId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transakcija", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transakcija_ParkingLokacija_ParkingLokacijaId",
                        column: x => x.ParkingLokacijaId,
                        principalTable: "ParkingLokacija",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transakcija_Vozilo_VoziloId",
                        column: x => x.VoziloId,
                        principalTable: "Vozilo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zahtjev",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoziloId = table.Column<int>(nullable: false),
                    VlasnikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zahtjev", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Zahtjev_Vlasnik_VlasnikId",
                        column: x => x.VlasnikId,
                        principalTable: "Vlasnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zahtjev_Vozilo_VoziloId",
                        column: x => x.VoziloId,
                        principalTable: "Vozilo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLokacija_CjenovnikId",
                table: "ParkingLokacija",
                column: "CjenovnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_ParkingLokacijaId",
                table: "Transakcija",
                column: "ParkingLokacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_VoziloId",
                table: "Transakcija",
                column: "VoziloId");

            migrationBuilder.CreateIndex(
                name: "IX_Vlasnik_ParkingLokacijaId",
                table: "Vlasnik",
                column: "ParkingLokacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vozilo_KorisnikId",
                table: "Vozilo",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Zahtjev_VlasnikId",
                table: "Zahtjev",
                column: "VlasnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Zahtjev_VoziloId",
                table: "Zahtjev",
                column: "VoziloId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.DropTable(
                name: "Transakcija");

            migrationBuilder.DropTable(
                name: "Zahtjev");

            migrationBuilder.DropTable(
                name: "Vlasnik");

            migrationBuilder.DropTable(
                name: "Vozilo");

            migrationBuilder.DropTable(
                name: "ParkingLokacija");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "Cjenovnik");
        }
    }
}
