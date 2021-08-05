using Microsoft.EntityFrameworkCore.Migrations;

namespace HelpDesk.Api.Migrations
{
    public partial class addPlanta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "planta_id",
                table: "ticket",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "planta_id",
                table: "ticket");
        }
    }
}
