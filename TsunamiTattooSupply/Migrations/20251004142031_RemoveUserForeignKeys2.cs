using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserForeignKeys2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_DeletedUserID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_EditUserID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DeletedUserID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_EditUserID",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedUserID",
                table: "Users",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EditUserID",
                table: "Users",
                column: "EditUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_DeletedUserID",
                table: "Users",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_EditUserID",
                table: "Users",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
