using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelpDesk.Api.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ticket",
                columns: table => new
                {
                    ticked_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoReporte = table.Column<int>(nullable: false),
                    EstadoTicket = table.Column<int>(nullable: false),
                    NombreReporta = table.Column<string>(nullable: true),
                    ComentarioAlta = table.Column<string>(nullable: true),
                    AsignadoA = table.Column<string>(nullable: true),
                    FechaAlta = table.Column<DateTime>(nullable: false),
                    FechaAsignado = table.Column<DateTime>(nullable: false),
                    FechaTermino = table.Column<DateTime>(nullable: false),
                    ComentarioAsignado = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket", x => x.ticked_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ticket");
        }
    }
}
