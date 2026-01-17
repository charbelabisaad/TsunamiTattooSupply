using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalFielsToProductTypesDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_Users_DeletedUserID",
                table: "ProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_Users_EditUserID",
                table: "ProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProudctTypes_Users_DeletedUserID",
                table: "ProudctTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProudctTypes_Users_EditUserID",
                table: "ProudctTypes");

            migrationBuilder.AlterColumn<int>(
                name: "EditUserID",
                table: "ProudctTypes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "ProudctTypes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "ProudctTypes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ProudctTypes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditDate",
                table: "ProudctTypes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EditUserID",
                table: "ProductDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "ProductDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "ProductDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ProductDetails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditDate",
                table: "ProductDetails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_Users_DeletedUserID",
                table: "ProductDetails",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_Users_EditUserID",
                table: "ProductDetails",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProudctTypes_Users_DeletedUserID",
                table: "ProudctTypes",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProudctTypes_Users_EditUserID",
                table: "ProudctTypes",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_Users_DeletedUserID",
                table: "ProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_Users_EditUserID",
                table: "ProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProudctTypes_Users_DeletedUserID",
                table: "ProudctTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProudctTypes_Users_EditUserID",
                table: "ProudctTypes");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "ProudctTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ProudctTypes");

            migrationBuilder.DropColumn(
                name: "EditDate",
                table: "ProudctTypes");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "EditDate",
                table: "ProductDetails");

            migrationBuilder.AlterColumn<int>(
                name: "EditUserID",
                table: "ProudctTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "ProudctTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EditUserID",
                table: "ProductDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "ProductDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_Users_DeletedUserID",
                table: "ProductDetails",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_Users_EditUserID",
                table: "ProductDetails",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProudctTypes_Users_DeletedUserID",
                table: "ProudctTypes",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProudctTypes_Users_EditUserID",
                table: "ProudctTypes",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
