using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class FixUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatedUserID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_EdtedUserID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_EdtedUserID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EdtedUserID",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EditUserID",
                table: "Users",
                column: "EditUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedUserID",
                table: "Users",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_EditUserID",
                table: "Users",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatedUserID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_EditUserID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_EditUserID",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "EdtedUserID",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EdtedUserID",
                table: "Users",
                column: "EdtedUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedUserID",
                table: "Users",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_EdtedUserID",
                table: "Users",
                column: "EdtedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
