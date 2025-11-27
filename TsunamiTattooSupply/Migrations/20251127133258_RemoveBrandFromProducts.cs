using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBrandFromProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandID",
                table: "Products",
                column: "BrandID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandID",
                table: "Products",
                column: "BrandID",
                principalTable: "Brands",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
