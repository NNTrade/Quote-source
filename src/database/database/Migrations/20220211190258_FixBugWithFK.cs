using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.Migrations
{
    public partial class FixBugWithFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quote_StockTimeFrame_StockTimeFrameId1",
                table: "Quote");

            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Market_MarketId1",
                table: "Stock");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTimeFrame_Stock_StockId1",
                table: "StockTimeFrame");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTimeFrame_TimeFrame_TimeFrameId1",
                table: "StockTimeFrame");

            migrationBuilder.DropIndex(
                name: "IX_StockTimeFrame_StockId1",
                table: "StockTimeFrame");

            migrationBuilder.DropIndex(
                name: "IX_StockTimeFrame_TimeFrameId1",
                table: "StockTimeFrame");

            migrationBuilder.DropIndex(
                name: "IX_Stock_MarketId1",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Quote_StockTimeFrameId1",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "StockId1",
                table: "StockTimeFrame");

            migrationBuilder.DropColumn(
                name: "TimeFrameId1",
                table: "StockTimeFrame");

            migrationBuilder.DropColumn(
                name: "MarketId1",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "StockTimeFrameId1",
                table: "Quote");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockId1",
                table: "StockTimeFrame",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeFrameId1",
                table: "StockTimeFrame",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MarketId1",
                table: "Stock",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StockTimeFrameId1",
                table: "Quote",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StockTimeFrame_StockId1",
                table: "StockTimeFrame",
                column: "StockId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockTimeFrame_TimeFrameId1",
                table: "StockTimeFrame",
                column: "TimeFrameId1");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_MarketId1",
                table: "Stock",
                column: "MarketId1");

            migrationBuilder.CreateIndex(
                name: "IX_Quote_StockTimeFrameId1",
                table: "Quote",
                column: "StockTimeFrameId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Quote_StockTimeFrame_StockTimeFrameId1",
                table: "Quote",
                column: "StockTimeFrameId1",
                principalTable: "StockTimeFrame",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Market_MarketId1",
                table: "Stock",
                column: "MarketId1",
                principalTable: "Market",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTimeFrame_Stock_StockId1",
                table: "StockTimeFrame",
                column: "StockId1",
                principalTable: "Stock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTimeFrame_TimeFrame_TimeFrameId1",
                table: "StockTimeFrame",
                column: "TimeFrameId1",
                principalTable: "TimeFrame",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
