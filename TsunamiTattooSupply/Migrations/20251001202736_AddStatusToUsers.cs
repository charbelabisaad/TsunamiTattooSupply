using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogIn",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "StatusID",
                table: "Users",
                type: "character varying(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    ID = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_StatusID",
                table: "Users",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Statuses_StatusID",
                table: "Users",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Statuses_StatusID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Users_StatusID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "LogIn",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
