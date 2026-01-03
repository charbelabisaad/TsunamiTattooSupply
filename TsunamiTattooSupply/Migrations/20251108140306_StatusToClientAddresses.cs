using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class StatusToClientAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedUserID",
                table: "ClientAddresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserID",
                table: "ClientAddresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EditUserID",
                table: "ClientAddresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StatusID",
                table: "ClientAddresses",
                type: "character varying(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAddresses_CreatedUserID",
                table: "ClientAddresses",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAddresses_DeletedUserID",
                table: "ClientAddresses",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAddresses_EditUserID",
                table: "ClientAddresses",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAddresses_StatusID",
                table: "ClientAddresses",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAddresses_Statuses_StatusID",
                table: "ClientAddresses",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAddresses_Users_CreatedUserID",
                table: "ClientAddresses",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAddresses_Users_DeletedUserID",
                table: "ClientAddresses",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAddresses_Users_EditUserID",
                table: "ClientAddresses",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAddresses_Statuses_StatusID",
                table: "ClientAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientAddresses_Users_CreatedUserID",
                table: "ClientAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientAddresses_Users_DeletedUserID",
                table: "ClientAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientAddresses_Users_EditUserID",
                table: "ClientAddresses");

            migrationBuilder.DropIndex(
                name: "IX_ClientAddresses_CreatedUserID",
                table: "ClientAddresses");

            migrationBuilder.DropIndex(
                name: "IX_ClientAddresses_DeletedUserID",
                table: "ClientAddresses");

            migrationBuilder.DropIndex(
                name: "IX_ClientAddresses_EditUserID",
                table: "ClientAddresses");

            migrationBuilder.DropIndex(
                name: "IX_ClientAddresses_StatusID",
                table: "ClientAddresses");

            migrationBuilder.DropColumn(
                name: "CreatedUserID",
                table: "ClientAddresses");

            migrationBuilder.DropColumn(
                name: "DeletedUserID",
                table: "ClientAddresses");

            migrationBuilder.DropColumn(
                name: "EditUserID",
                table: "ClientAddresses");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "ClientAddresses");
        }
    }
}
