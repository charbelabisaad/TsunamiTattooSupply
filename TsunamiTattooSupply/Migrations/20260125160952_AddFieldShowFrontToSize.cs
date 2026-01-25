using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldShowFrontToSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowFront",
                table: "Sizes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowFront",
                table: "Sizes");
        }
    }
}
