using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Migrations
{
    public partial class V24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdLevel",
                table: "Costs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Costs_IdLevel",
                table: "Costs",
                column: "IdLevel");

            migrationBuilder.AddForeignKey(
                name: "FK_Costs_Levels_IdLevel",
                table: "Costs",
                column: "IdLevel",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Costs_Levels_IdLevel",
                table: "Costs");

            migrationBuilder.DropIndex(
                name: "IX_Costs_IdLevel",
                table: "Costs");

            migrationBuilder.DropColumn(
                name: "IdLevel",
                table: "Costs");
        }
    }
}
