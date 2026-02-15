using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class DeleteUsersIDFromCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_CreatedUserID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_DeletedUserID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_EditUserID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Clients_ClientID",
                table: "Tables");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Statuses_StatusID",
                table: "Tables");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Users_CreatedUserID",
                table: "Tables");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Users_DeletedUserID",
                table: "Tables");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Users_EditUserID",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CreatedUserID",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_DeletedUserID",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_EditUserID",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tables",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "CreatedUserID",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "DeletedUserID",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "EditUserID",
                table: "Carts");
           
     
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
             
            migrationBuilder.AddColumn<int>(
                name: "CreatedUserID",
                table: "Carts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Carts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserID",
                table: "Carts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EditUserID",
                table: "Carts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tables",
                table: "Tables",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CreatedUserID",
                table: "Carts",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_DeletedUserID",
                table: "Carts",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_EditUserID",
                table: "Carts",
                column: "EditUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_CreatedUserID",
                table: "Carts",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_DeletedUserID",
                table: "Carts",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_EditUserID",
                table: "Carts",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Clients_ClientID",
                table: "Tables",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Statuses_StatusID",
                table: "Tables",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Users_CreatedUserID",
                table: "Tables",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Users_DeletedUserID",
                table: "Tables",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Users_EditUserID",
                table: "Tables",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
