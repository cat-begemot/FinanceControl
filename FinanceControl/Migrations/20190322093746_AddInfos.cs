using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceControl.Migrations
{
    public partial class AddInfos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "help");

            migrationBuilder.RenameTable(
                name: "Helpers",
                schema: "Help",
                newName: "Helpers",
                newSchema: "help");

            migrationBuilder.CreateTable(
                name: "infos",
                schema: "help",
                columns: table => new
                {
                    InfoId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    header = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos", x => x.InfoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "infos",
                schema: "help");

            migrationBuilder.EnsureSchema(
                name: "Help");

            migrationBuilder.RenameTable(
                name: "Helpers",
                schema: "help",
                newName: "Helpers",
                newSchema: "Help");
        }
    }
}
