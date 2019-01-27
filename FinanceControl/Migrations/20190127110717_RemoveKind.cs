using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceControl.Migrations
{
    public partial class RemoveKind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kinds");

            migrationBuilder.DropColumn(
                name: "KindId",
                table: "Items");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ItemId",
                table: "Transactions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_GroupId",
                table: "Items",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Groups_GroupId",
                table: "Items",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Items_ItemId",
                table: "Transactions",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Groups_GroupId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Items_ItemId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ItemId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Items_GroupId",
                table: "Items");

            migrationBuilder.AddColumn<long>(
                name: "KindId",
                table: "Items",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Kinds",
                columns: table => new
                {
                    KindId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<long>(nullable: false),
                    GroupId = table.Column<long>(nullable: false),
                    Name = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kinds", x => x.KindId);
                });
        }
    }
}
