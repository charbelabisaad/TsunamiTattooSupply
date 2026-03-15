using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusIDToPageLoacations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusID",
                table: "PageLocations",
                type: "character varying(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PageLocations_StatusID",
                table: "PageLocations",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_PageLocations_Statuses_StatusID",
                table: "PageLocations",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageLocations_Statuses_StatusID",
                table: "PageLocations");

            migrationBuilder.DropIndex(
                name: "IX_PageLocations_StatusID",
                table: "PageLocations");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "PageLocations");
        }
    }
}
