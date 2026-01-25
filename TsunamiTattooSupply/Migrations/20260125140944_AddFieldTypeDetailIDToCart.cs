using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldTypeDetailIDToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductDetailID",
                table: "Carts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductTypeID",
                table: "Carts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductDetailID",
                table: "Carts",
                column: "ProductDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductTypeID",
                table: "Carts",
                column: "ProductTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_ProductDetails_ProductDetailID",
                table: "Carts",
                column: "ProductDetailID",
                principalTable: "ProductDetails",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_ProudctTypes_ProductTypeID",
                table: "Carts",
                column: "ProductTypeID",
                principalTable: "ProudctTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_ProductDetails_ProductDetailID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_ProudctTypes_ProductTypeID",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_ProductDetailID",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_ProductTypeID",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ProductDetailID",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ProductTypeID",
                table: "Carts");
        }
    }
}
