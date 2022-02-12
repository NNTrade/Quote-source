using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.Migrations
{
    public partial class renameTimeframe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TimeFrame",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Day");

            migrationBuilder.UpdateData(
                table: "TimeFrame",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Week");

            migrationBuilder.UpdateData(
                table: "TimeFrame",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Month");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TimeFrame",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Daily");

            migrationBuilder.UpdateData(
                table: "TimeFrame",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Weekly");

            migrationBuilder.UpdateData(
                table: "TimeFrame",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Monthly");
        }
    }
}
