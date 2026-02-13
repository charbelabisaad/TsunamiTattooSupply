using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTypeDetailIDToStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductDetailID",
                table: "Stocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductTypeID",
                table: "Stocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductDetailID",
                table: "Stocks",
                column: "ProductDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductTypeID",
                table: "Stocks",
                column: "ProductTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_ProductDetails_ProductDetailID",
                table: "Stocks",
                column: "ProductDetailID",
                principalTable: "ProductDetails",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_ProudctTypes_ProductTypeID",
                table: "Stocks",
                column: "ProductTypeID",
                principalTable: "ProudctTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_ProductDetails_ProductDetailID",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_ProudctTypes_ProductTypeID",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_ProductDetailID",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_ProductTypeID",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "ProductDetailID",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "ProductTypeID",
                table: "Stocks");
        }
    }
}
