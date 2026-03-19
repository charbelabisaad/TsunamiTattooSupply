using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddTypesDetailsLabelToSubCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DetailsLabel",
                table: "SubCategories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypesLabel",
                table: "SubCategories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetailsLabel",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "TypesLabel",
                table: "SubCategories");
        }
    }
}
