using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBrandIDToGroupIDInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_GroupID",
                table: "Products",
                column: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Groups_GroupID",
                table: "Products",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Groups_GroupID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_GroupID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "Products");
        }
    }
}
