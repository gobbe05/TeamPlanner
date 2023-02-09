using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamPlanner.Migrations
{
    public partial class GroupChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeLeft",
                table: "Groups",
                newName: "MoneyUsed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoneyUsed",
                table: "Groups",
                newName: "TimeLeft");
        }
    }
}
