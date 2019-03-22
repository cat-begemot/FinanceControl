using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceControl.Migrations
{
    public partial class AddInfo_Rev1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "infos",
                schema: "help",
                newName: "Infos",
                newSchema: "help");

            migrationBuilder.RenameColumn(
                name: "InfoId",
                schema: "help",
                table: "Infos",
                newName: "infoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Infos",
                schema: "help",
                newName: "infos",
                newSchema: "help");

            migrationBuilder.RenameColumn(
                name: "infoId",
                schema: "help",
                table: "infos",
                newName: "InfoId");
        }
    }
}
