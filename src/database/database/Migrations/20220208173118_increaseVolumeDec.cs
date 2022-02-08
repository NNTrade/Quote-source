using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database.Migrations
{
    public partial class increaseVolumeDec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Volume",
                table: "Quote",
                type: "numeric(19,9)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(15,9)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Volume",
                table: "Quote",
                type: "numeric(15,9)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,9)");
        }
    }
}
