using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Migrations
{
    public partial class V16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisterCode",
                table: "Students",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterCode",
                table: "Students");
        }
    }
}
