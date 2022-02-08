using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace database.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Market",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Market", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeFrame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeFrame", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MarketId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FinamId = table.Column<int>(type: "integer", nullable: false),
                    MarketId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Market_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stock_Market_MarketId1",
                        column: x => x.MarketId1,
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTimeFrame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StockId = table.Column<int>(type: "integer", nullable: false),
                    TimeFrameId = table.Column<int>(type: "integer", nullable: false),
                    LoadedFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LoadedTill = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StockId1 = table.Column<int>(type: "integer", nullable: false),
                    TimeFrameId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTimeFrame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTimeFrame_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTimeFrame_Stock_StockId1",
                        column: x => x.StockId1,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTimeFrame_TimeFrame_TimeFrameId",
                        column: x => x.TimeFrameId,
                        principalTable: "TimeFrame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTimeFrame_TimeFrame_TimeFrameId1",
                        column: x => x.TimeFrameId1,
                        principalTable: "TimeFrame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandleStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Open = table.Column<decimal>(type: "numeric(15,9)", nullable: false),
                    High = table.Column<decimal>(type: "numeric(15,9)", nullable: false),
                    Low = table.Column<decimal>(type: "numeric(15,9)", nullable: false),
                    Close = table.Column<decimal>(type: "numeric(15,9)", nullable: false),
                    Volume = table.Column<decimal>(type: "numeric(15,9)", nullable: false),
                    StockTimeFrameId = table.Column<int>(type: "integer", nullable: false),
                    StockTimeFrameId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quote_StockTimeFrame_StockTimeFrameId",
                        column: x => x.StockTimeFrameId,
                        principalTable: "StockTimeFrame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quote_StockTimeFrame_StockTimeFrameId1",
                        column: x => x.StockTimeFrameId1,
                        principalTable: "StockTimeFrame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Market",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Forex" },
                    { 2, "UsaStock" },
                    { 3, "MmvbStock" },
                    { 4, "CryptoCurrency" }
                });

            migrationBuilder.InsertData(
                table: "TimeFrame",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Minute" },
                    { 2, "Minute5" },
                    { 3, "Minute10" },
                    { 4, "Minute15" },
                    { 5, "Minute30" },
                    { 6, "Hour" },
                    { 8, "Daily" },
                    { 9, "Weekly" },
                    { 10, "Monthly" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quote_StockTimeFrameId_CandleStart",
                table: "Quote",
                columns: new[] { "StockTimeFrameId", "CandleStart" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quote_StockTimeFrameId1",
                table: "Quote",
                column: "StockTimeFrameId1");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_MarketId_Code",
                table: "Stock",
                columns: new[] { "MarketId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_Stock_MarketId1",
                table: "Stock",
                column: "MarketId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockTimeFrame_StockId_TimeFrameId",
                table: "StockTimeFrame",
                columns: new[] { "StockId", "TimeFrameId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockTimeFrame_StockId1",
                table: "StockTimeFrame",
                column: "StockId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockTimeFrame_TimeFrameId",
                table: "StockTimeFrame",
                column: "TimeFrameId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTimeFrame_TimeFrameId1",
                table: "StockTimeFrame",
                column: "TimeFrameId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quote");

            migrationBuilder.DropTable(
                name: "StockTimeFrame");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "TimeFrame");

            migrationBuilder.DropTable(
                name: "Market");
        }
    }
}
