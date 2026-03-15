using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddRankToPageLoacations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PageLocations",
                newName: "ID");

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "PageLocations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "PageLocations");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PageLocations",
                newName: "Id");
        }
    }
}
