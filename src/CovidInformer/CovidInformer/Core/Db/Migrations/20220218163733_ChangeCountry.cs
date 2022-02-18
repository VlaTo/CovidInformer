using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidInformer.Core.Db.Migrations
{
    public partial class ChangeCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Country",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "NativeName",
                table: "Country",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "Country",
                type: "char(2)",
                maxLength: 2,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NativeName",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "Country");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Country",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
