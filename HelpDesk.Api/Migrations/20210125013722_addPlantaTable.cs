using Microsoft.EntityFrameworkCore.Migrations;

namespace HelpDesk.Api.Migrations
{
    public partial class addPlantaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "planta",
                columns: table => new
                {
                    planta_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    planta_nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_planta", x => x.planta_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "planta");
        }
    }
}
