using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidInformer.Core.Db.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Update",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Update", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Counter",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<ulong>(nullable: false),
                    CountryRef = table.Column<long>(nullable: false),
                    UpdateRef = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Counter_Country_CountryRef",
                        column: x => x.CountryRef,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Counter_Update_UpdateRef",
                        column: x => x.UpdateRef,
                        principalTable: "Update",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Counter_CountryRef",
                table: "Counter",
                column: "CountryRef");

            migrationBuilder.CreateIndex(
                name: "IX_Counter_UpdateRef",
                table: "Counter",
                column: "UpdateRef");

            migrationBuilder.CreateIndex(
                name: "IX_Country_DisplayName",
                table: "Country",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_Id",
                table: "Country",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Update_Updated",
                table: "Update",
                column: "Updated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Counter");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Update");
        }
    }
}
