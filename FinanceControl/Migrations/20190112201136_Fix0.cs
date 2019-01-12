using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceControl.Migrations
{
    public partial class Fix0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Items_ItemId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Kinds_Items_ItemId",
                table: "Kinds");

            migrationBuilder.DropIndex(
                name: "IX_Kinds_ItemId",
                table: "Kinds");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ItemId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Kinds");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Groups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ItemId",
                table: "Kinds",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ItemId",
                table: "Groups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kinds_ItemId",
                table: "Kinds",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ItemId",
                table: "Groups",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Items_ItemId",
                table: "Groups",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kinds_Items_ItemId",
                table: "Kinds",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
