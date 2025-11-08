using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductColorFromProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsImages_ProductsColors_ProductColorID",
                table: "ProductsImages");

            migrationBuilder.RenameColumn(
                name: "ProductColorID",
                table: "ProductsImages",
                newName: "ColorID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsImages_ProductColorID",
                table: "ProductsImages",
                newName: "IX_ProductsImages_ColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsImages_Colors_ColorID",
                table: "ProductsImages",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsImages_Colors_ColorID",
                table: "ProductsImages");

            migrationBuilder.RenameColumn(
                name: "ColorID",
                table: "ProductsImages",
                newName: "ProductColorID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsImages_ColorID",
                table: "ProductsImages",
                newName: "IX_ProductsImages_ProductColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsImages_ProductsColors_ProductColorID",
                table: "ProductsImages",
                column: "ProductColorID",
                principalTable: "ProductsColors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
