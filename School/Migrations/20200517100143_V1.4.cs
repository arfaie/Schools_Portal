using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Migrations
{
    public partial class V14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Levels");
        }
    }
}
