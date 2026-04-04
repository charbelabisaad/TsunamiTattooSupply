using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddParentIDToColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentID",
                table: "Colors",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ParentID",
                table: "Colors",
                column: "ParentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Colors_ParentID",
                table: "Colors",
                column: "ParentID",
                principalTable: "Colors",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Colors_ParentID",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Colors_ParentID",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ParentID",
                table: "Colors");
        }
    }
}
