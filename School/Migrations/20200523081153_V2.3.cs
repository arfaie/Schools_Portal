using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Migrations
{
    public partial class V23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nationalCodeImage",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reportImage",
                table: "Students",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nationalCodeImage",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "reportImage",
                table: "Students");
        }
    }
}
