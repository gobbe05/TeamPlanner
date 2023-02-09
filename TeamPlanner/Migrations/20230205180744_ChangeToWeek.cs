using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamPlanner.Migrations
{
    public partial class ChangeToWeek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "UnavailableTimes",
                newName: "Week");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Times",
                newName: "Week");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Week",
                table: "UnavailableTimes",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Week",
                table: "Times",
                newName: "Date");
        }
    }
}
