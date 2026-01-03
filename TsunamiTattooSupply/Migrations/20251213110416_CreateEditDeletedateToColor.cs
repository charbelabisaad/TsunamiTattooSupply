using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class CreateEditDeletedateToColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Users_DeletedUserID",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Users_EditUserID",
                table: "Colors");

            migrationBuilder.AlterColumn<int>(
                name: "EditUserID",
                table: "Colors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "Colors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Colors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Colors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditDate",
                table: "Colors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Users_DeletedUserID",
                table: "Colors",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Users_EditUserID",
                table: "Colors",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Users_DeletedUserID",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Users_EditUserID",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "EditDate",
                table: "Colors");

            migrationBuilder.AlterColumn<int>(
                name: "EditUserID",
                table: "Colors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "Colors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Users_DeletedUserID",
                table: "Colors",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Users_EditUserID",
                table: "Colors",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
