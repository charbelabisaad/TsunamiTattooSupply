using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupToBannerPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "BannersPages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_GroupID",
                table: "BannersPages",
                column: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_BannersPages_Groups_GroupID",
                table: "BannersPages",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannersPages_Groups_GroupID",
                table: "BannersPages");

            migrationBuilder.DropIndex(
                name: "IX_BannersPages_GroupID",
                table: "BannersPages");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "BannersPages");
        }
    }
}
