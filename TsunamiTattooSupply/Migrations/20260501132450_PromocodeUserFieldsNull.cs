using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class PromocodeUserFieldsNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Users_CreatedUserID",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Users_DeletedUserID",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Users_EditUserID",
                table: "PromoCodes");

            migrationBuilder.AlterColumn<int>(
                name: "EditUserID",
                table: "PromoCodes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "PromoCodes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "PromoCodes",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserID",
                table: "PromoCodes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Users_CreatedUserID",
                table: "PromoCodes",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Users_DeletedUserID",
                table: "PromoCodes",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Users_EditUserID",
                table: "PromoCodes",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Users_CreatedUserID",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Users_DeletedUserID",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Users_EditUserID",
                table: "PromoCodes");

            migrationBuilder.AlterColumn<int>(
                name: "EditUserID",
                table: "PromoCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "PromoCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "PromoCodes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedUserID",
                table: "PromoCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Users_CreatedUserID",
                table: "PromoCodes",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Users_DeletedUserID",
                table: "PromoCodes",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Users_EditUserID",
                table: "PromoCodes",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
