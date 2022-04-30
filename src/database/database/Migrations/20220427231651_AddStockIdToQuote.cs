using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.Migrations
{
    public partial class AddStockIdToQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "Quote",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Quote_StockId",
                table: "Quote",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quote_Stock_StockId",
                table: "Quote",
                column: "StockId",
                principalTable: "Stock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quote_Stock_StockId",
                table: "Quote");

            migrationBuilder.DropIndex(
                name: "IX_Quote_StockId",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "Quote");
        }
    }
}
