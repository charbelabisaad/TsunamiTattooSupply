using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupToBannerMobile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannersMobiles_Categories_CategoryID",
                table: "BannersMobiles");

            migrationBuilder.DropForeignKey(
                name: "FK_BannersMobiles_Products_ProductID",
                table: "BannersMobiles");

            migrationBuilder.DropForeignKey(
                name: "FK_BannersMobiles_SubCategories_SubCategoryID",
                table: "BannersMobiles");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryID",
                table: "BannersMobiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "BannersMobiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "BannersMobiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "BannersMobiles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannersMobiles_GroupID",
                table: "BannersMobiles",
                column: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_BannersMobiles_Categories_CategoryID",
                table: "BannersMobiles",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BannersMobiles_Groups_GroupID",
                table: "BannersMobiles",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BannersMobiles_Products_ProductID",
                table: "BannersMobiles",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BannersMobiles_SubCategories_SubCategoryID",
                table: "BannersMobiles",
                column: "SubCategoryID",
                principalTable: "SubCategories",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannersMobiles_Categories_CategoryID",
                table: "BannersMobiles");

            migrationBuilder.DropForeignKey(
                name: "FK_BannersMobiles_Groups_GroupID",
                table: "BannersMobiles");

            migrationBuilder.DropForeignKey(
                name: "FK_BannersMobiles_Products_ProductID",
                table: "BannersMobiles");

            migrationBuilder.DropForeignKey(
                name: "FK_BannersMobiles_SubCategories_SubCategoryID",
                table: "BannersMobiles");

            migrationBuilder.DropIndex(
                name: "IX_BannersMobiles_GroupID",
                table: "BannersMobiles");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "BannersMobiles");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryID",
                table: "BannersMobiles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "BannersMobiles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "BannersMobiles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BannersMobiles_Categories_CategoryID",
                table: "BannersMobiles",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BannersMobiles_Products_ProductID",
                table: "BannersMobiles",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BannersMobiles_SubCategories_SubCategoryID",
                table: "BannersMobiles",
                column: "SubCategoryID",
                principalTable: "SubCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
