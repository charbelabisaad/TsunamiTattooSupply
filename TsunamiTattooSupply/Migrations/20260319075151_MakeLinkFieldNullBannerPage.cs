using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class MakeLinkFieldNullBannerPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_CategoryID",
                table: "BannersPages",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_ProductID",
                table: "BannersPages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_SubCategoryID",
                table: "BannersPages",
                column: "SubCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_BannersPages_Categories_CategoryID",
                table: "BannersPages",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BannersPages_Products_ProductID",
                table: "BannersPages",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BannersPages_SubCategories_SubCategoryID",
                table: "BannersPages",
                column: "SubCategoryID",
                principalTable: "SubCategories",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannersPages_Categories_CategoryID",
                table: "BannersPages");

            migrationBuilder.DropForeignKey(
                name: "FK_BannersPages_Products_ProductID",
                table: "BannersPages");

            migrationBuilder.DropForeignKey(
                name: "FK_BannersPages_SubCategories_SubCategoryID",
                table: "BannersPages");

            migrationBuilder.DropIndex(
                name: "IX_BannersPages_CategoryID",
                table: "BannersPages");

            migrationBuilder.DropIndex(
                name: "IX_BannersPages_ProductID",
                table: "BannersPages");

            migrationBuilder.DropIndex(
                name: "IX_BannersPages_SubCategoryID",
                table: "BannersPages");
        }
    }
}
