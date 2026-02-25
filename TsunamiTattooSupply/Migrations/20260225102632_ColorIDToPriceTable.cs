using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class ColorIDToPriceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorID",
                table: "Prices",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ColorID",
                table: "Prices",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ProductDetailID",
                table: "Prices",
                column: "ProductDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ProductTypeID",
                table: "Prices",
                column: "ProductTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Colors_ColorID",
                table: "Prices",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Colors_ColorID",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_ColorID",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_ProductDetailID",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_ProductTypeID",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "ColorID",
                table: "Prices");
        }
    }
}
