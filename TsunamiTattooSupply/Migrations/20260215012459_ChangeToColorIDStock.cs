using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToColorIDStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_ProductsColors_ProductColorID",
                table: "Stocks");

            migrationBuilder.RenameColumn(
                name: "ProductColorID",
                table: "Stocks",
                newName: "ColorID");

            migrationBuilder.RenameIndex(
                name: "IX_Stocks_ProductColorID",
                table: "Stocks",
                newName: "IX_Stocks_ColorID");

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "Carts",
                type: "numeric(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Colors_ColorID",
                table: "Stocks",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Colors_ColorID",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ColorID",
                table: "Stocks",
                newName: "ProductColorID");

            migrationBuilder.RenameIndex(
                name: "IX_Stocks_ColorID",
                table: "Stocks",
                newName: "IX_Stocks_ProductColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_ProductsColors_ProductColorID",
                table: "Stocks",
                column: "ProductColorID",
                principalTable: "ProductsColors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
