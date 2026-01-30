using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class DropCreateEditDeleteUserFromClientAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAddresses_Users_DeletedUserID",
                table: "ClientAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientAddresses_Users_EditUserID",
                table: "ClientAddresses");

            migrationBuilder.DropIndex(
                name: "IX_ClientAddresses_DeletedUserID",
                table: "ClientAddresses");

            migrationBuilder.DropIndex(
                name: "IX_ClientAddresses_EditUserID",
                table: "ClientAddresses");

            migrationBuilder.DropColumn(
                name: "DeletedUserID",
                table: "ClientAddresses");

            migrationBuilder.DropColumn(
                name: "EditUserID",
                table: "ClientAddresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeletedUserID",
                table: "ClientAddresses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EditUserID",
                table: "ClientAddresses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientAddresses_DeletedUserID",
                table: "ClientAddresses",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAddresses_EditUserID",
                table: "ClientAddresses",
                column: "EditUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAddresses_Users_DeletedUserID",
                table: "ClientAddresses",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAddresses_Users_EditUserID",
                table: "ClientAddresses",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
