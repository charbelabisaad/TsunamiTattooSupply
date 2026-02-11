using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryCurrencyToCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryID",
                table: "Carts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyID",
                table: "Carts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CountryID",
                table: "Carts",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CurrencyID",
                table: "Carts",
                column: "CurrencyID");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Colors_CurrencyID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Sizes_CountryID",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CountryID",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CurrencyID",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CountryID",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CurrencyID",
                table: "Carts");
        }
    }
}
