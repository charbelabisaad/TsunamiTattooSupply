using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryIDToCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryID",
                table: "Currencies",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CountryID",
                table: "Currencies",
                column: "CountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Countries_CountryID",
                table: "Currencies",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Countries_CountryID",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_CountryID",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "CountryID",
                table: "Currencies");
        }
    }
}
