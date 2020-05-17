using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Migrations
{
    public partial class V17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "02e1122fc0d3a65db4cdc514");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "096a4bff94237b95c131ee9d");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "1c76d0b4dde0e7a116273075");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "239443dfa6a286607d777e71");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "458813190d896a4dd601ae18");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "4d414cba8d6965cc0a482381");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "56b04a2c864e3706727f7d41");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "72d54246a837967a52bfe5ad");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "738e40ca96f4308399678ab5");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "7a7c4e6aade4c0a7636508ae");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f9a");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "cfd14d118be838a685e20e3e");

            migrationBuilder.InsertData(
                table: "Levels",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { "afde4ad493d2eb181cb87f01", "اول" },
                    { "afde4ad493d2eb181cb87f02", "دوم" },
                    { "afde4ad493d2eb181cb87f03", "سوم" },
                    { "afde4ad493d2eb181cb87f04", "چهارم" },
                    { "afde4ad493d2eb181cb87f05", "پنجم" },
                    { "afde4ad493d2eb181cb87f06", "ششم" },
                    { "afde4ad493d2eb181cb87f07", "هفتم" },
                    { "afde4ad493d2eb181cb87f08", "هشتم" },
                    { "afde4ad493d2eb181cb87f09", "نهم" },
                    { "afde4ad493d2eb181cb87f10", "دهم" },
                    { "afde4ad493d2eb181cb87f11", "یازدهم" },
                    { "afde4ad493d2eb181cb87f12", "دوازدهم" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f01");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f02");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f03");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f04");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f05");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f06");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f07");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f08");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f09");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f10");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f11");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: "afde4ad493d2eb181cb87f12");

            migrationBuilder.InsertData(
                table: "Levels",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { "afde4ad493d2eb181cb87f9a", "اول" },
                    { "458813190d896a4dd601ae18", "دوم" },
                    { "7a7c4e6aade4c0a7636508ae", "سوم" },
                    { "239443dfa6a286607d777e71", "چهارم" },
                    { "1c76d0b4dde0e7a116273075", "پنجم" },
                    { "56b04a2c864e3706727f7d41", "ششم" },
                    { "72d54246a837967a52bfe5ad", "هفتم" },
                    { "738e40ca96f4308399678ab5", "هشتم" },
                    { "02e1122fc0d3a65db4cdc514", "نهم" },
                    { "cfd14d118be838a685e20e3e", "دهم" },
                    { "096a4bff94237b95c131ee9d", "یازدهم" },
                    { "4d414cba8d6965cc0a482381", "دوازدهم" }
                });
        }
    }
}
