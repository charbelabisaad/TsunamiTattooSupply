using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class ChangeActiveToStatusIDCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "StatusID",
                table: "Categories",
                type: "character varying(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_StatusID",
                table: "Categories",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Statuses_StatusID",
                table: "Categories",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Statuses_StatusID",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_StatusID",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "Categories");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
