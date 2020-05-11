using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Migrations
{
    public partial class V12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationTypes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factors",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    IdUser = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FactorCode = table.Column<string>(nullable: true),
                    IsPaid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factors_AspNetUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Years",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Years", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FatherName = table.Column<string>(nullable: true),
                    BtirhPlace = table.Column<string>(nullable: true),
                    IdentityCardPlace = table.Column<string>(nullable: true),
                    RightOrLeft = table.Column<bool>(nullable: false),
                    OldSchool = table.Column<string>(nullable: true),
                    LastYearAvreage = table.Column<float>(nullable: false),
                    FatherJob = table.Column<string>(nullable: true),
                    IdFatherEdu = table.Column<string>(nullable: true),
                    MotherJob = table.Column<string>(nullable: true),
                    IdMotherEdu = table.Column<string>(nullable: true),
                    IsDivorced = table.Column<bool>(nullable: false),
                    FatherJobAddress = table.Column<string>(nullable: true),
                    FatherJobTell = table.Column<string>(nullable: true),
                    MotherJobTell = table.Column<string>(nullable: true),
                    MotherJobAddress = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    FatherMobile = table.Column<string>(nullable: true),
                    MotherMobile = table.Column<string>(nullable: true),
                    IdUser = table.Column<string>(nullable: true),
                    IsPreSubmit = table.Column<bool>(nullable: false),
                    SubmitDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_EducationTypes_IdFatherEdu",
                        column: x => x.IdFatherEdu,
                        principalTable: "EducationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_EducationTypes_IdMotherEdu",
                        column: x => x.IdMotherEdu,
                        principalTable: "EducationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    FactorId = table.Column<string>(nullable: true),
                    TransactionNumber = table.Column<string>(nullable: true),
                    TransactionStatus = table.Column<bool>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Factors_FactorId",
                        column: x => x.FactorId,
                        principalTable: "Factors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Costs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    IdYear = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Costs_Years_IdYear",
                        column: x => x.IdYear,
                        principalTable: "Years",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactorItems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    FactorId = table.Column<string>(nullable: true),
                    CostId = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactorItems_Costs_CostId",
                        column: x => x.CostId,
                        principalTable: "Costs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FactorItems_Factors_FactorId",
                        column: x => x.FactorId,
                        principalTable: "Factors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "EducationTypes",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { "6f9c65d681937c32dafcec01", "زیر دیپلم" },
                    { "6f9c65d681937c32dafcec03", "دیپلم" },
                    { "6f9c65d681937c32dafcec05", "فوق دیپلم" },
                    { "6f9c65d681937c32dafcec06", "لیسانس" },
                    { "6f9c65d681937c32dafcec07", "فوق لیسانس" },
                    { "6f9c65d681937c32dafcec08", "دکتری" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Costs_IdYear",
                table: "Costs",
                column: "IdYear");

            migrationBuilder.CreateIndex(
                name: "IX_FactorItems_CostId",
                table: "FactorItems",
                column: "CostId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorItems_FactorId",
                table: "FactorItems",
                column: "FactorId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_IdUser",
                table: "Factors",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FactorId",
                table: "Payments",
                column: "FactorId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_IdFatherEdu",
                table: "Students",
                column: "IdFatherEdu");

            migrationBuilder.CreateIndex(
                name: "IX_Students_IdMotherEdu",
                table: "Students",
                column: "IdMotherEdu");

            migrationBuilder.CreateIndex(
                name: "IX_Students_IdUser",
                table: "Students",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactorItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Costs");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "EducationTypes");

            migrationBuilder.DropTable(
                name: "Years");
        }
    }
}
