using Microsoft.EntityFrameworkCore.Migrations;

namespace HelpDesk.Api.Migrations
{
    public partial class AddImageUrlTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "ticket",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "ticket");
        }
    }
}
