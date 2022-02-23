using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidInformer.Core.Db.Migrations
{
    public partial class CountryChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Country_DisplayName",
                table: "Country");

            migrationBuilder.RenameColumn(
                name: "RegionName",
                table: "Country",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "Country",
                newName: "CountryName");

            migrationBuilder.CreateIndex(
                name: "IX_Country_NativeName",
                table: "Country",
                column: "NativeName");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Region",
                table: "Country",
                column: "Region",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_Region_CountryName",
                table: "Country",
                columns: new[] { "Region", "CountryName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Country_NativeName",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Country_Region",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Country_Region_CountryName",
                table: "Country");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "Country",
                newName: "RegionName");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "Country",
                newName: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Country_DisplayName",
                table: "Country",
                column: "DisplayName",
                unique: true);
        }
    }
}
