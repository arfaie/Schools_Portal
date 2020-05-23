using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Migrations
{
    public partial class V21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MerchentCode",
                table: "Settings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MerchentCode",
                table: "Settings");
        }
    }
}
