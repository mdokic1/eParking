using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EParkingOOAD.Migrations
{
    public partial class izmjenaDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumRegistracije",
                table: "Vozilo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingLokacija_Vlasnik_VlasnikId",
                table: "ParkingLokacija");

            migrationBuilder.DropIndex(
                name: "IX_ParkingLokacija_VlasnikId",
                table: "ParkingLokacija");

            migrationBuilder.DropColumn(
                name: "DatumRegistracije",
                table: "Vozilo");

            migrationBuilder.DropColumn(
                name: "VlasnikId",
                table: "ParkingLokacija");

            migrationBuilder.AddColumn<int>(
                name: "ParkingLokacijaId",
                table: "Vlasnik",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vlasnik_ParkingLokacijaId",
                table: "Vlasnik",
                column: "ParkingLokacijaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vlasnik_ParkingLokacija_ParkingLokacijaId",
                table: "Vlasnik",
                column: "ParkingLokacijaId",
                principalTable: "ParkingLokacija",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
