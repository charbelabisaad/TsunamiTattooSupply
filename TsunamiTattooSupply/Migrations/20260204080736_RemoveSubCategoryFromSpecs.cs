using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSubCategoryFromSpecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specs_SubCategories_SubCategoryID",
                table: "Specs");

            migrationBuilder.DropIndex(
                name: "IX_Specs_SubCategoryID",
                table: "Specs");

            migrationBuilder.DropColumn(
                name: "SubCategoryID",
                table: "Specs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubCategoryID",
                table: "Specs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Specs_SubCategoryID",
                table: "Specs",
                column: "SubCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Specs_SubCategories_SubCategoryID",
                table: "Specs",
                column: "SubCategoryID",
                principalTable: "SubCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
