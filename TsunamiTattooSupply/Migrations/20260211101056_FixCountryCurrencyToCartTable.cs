using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class FixCountryCurrencyToCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Colors_CurrencyID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Sizes_CountryID",
                table: "Carts");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Countries_CountryID",
                table: "Carts",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Currencies_CurrencyID",
                table: "Carts",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Countries_CountryID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Currencies_CurrencyID",
                table: "Carts");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Colors_CurrencyID",
                table: "Carts",
                column: "CurrencyID",
                principalTable: "Colors",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Sizes_CountryID",
                table: "Carts",
                column: "CountryID",
                principalTable: "Sizes",
                principalColumn: "ID");
        }
    }
}
