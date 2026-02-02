using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class CorrectSpecsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsSpecs_Sepcs_SpecID",
                table: "ProductsSpecs");

            migrationBuilder.DropForeignKey(
                name: "FK_Sepcs_Statuses_StatusID",
                table: "Sepcs");

            migrationBuilder.DropForeignKey(
                name: "FK_Sepcs_Users_CreatedUserID",
                table: "Sepcs");

            migrationBuilder.DropForeignKey(
                name: "FK_Sepcs_Users_DeletedUserID",
                table: "Sepcs");

            migrationBuilder.DropForeignKey(
                name: "FK_Sepcs_Users_EditUserID",
                table: "Sepcs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sepcs",
                table: "Sepcs");

            migrationBuilder.RenameTable(
                name: "Sepcs",
                newName: "Specs");

            migrationBuilder.RenameIndex(
                name: "IX_Sepcs_StatusID",
                table: "Specs",
                newName: "IX_Specs_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Sepcs_EditUserID",
                table: "Specs",
                newName: "IX_Specs_EditUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Sepcs_DeletedUserID",
                table: "Specs",
                newName: "IX_Specs_DeletedUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Sepcs_CreatedUserID",
                table: "Specs",
                newName: "IX_Specs_CreatedUserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specs",
                table: "Specs",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsSpecs_Specs_SpecID",
                table: "ProductsSpecs",
                column: "SpecID",
                principalTable: "Specs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specs_Statuses_StatusID",
                table: "Specs",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specs_Users_CreatedUserID",
                table: "Specs",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specs_Users_DeletedUserID",
                table: "Specs",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Specs_Users_EditUserID",
                table: "Specs",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsSpecs_Specs_SpecID",
                table: "ProductsSpecs");

            migrationBuilder.DropForeignKey(
                name: "FK_Specs_Statuses_StatusID",
                table: "Specs");

            migrationBuilder.DropForeignKey(
                name: "FK_Specs_Users_CreatedUserID",
                table: "Specs");

            migrationBuilder.DropForeignKey(
                name: "FK_Specs_Users_DeletedUserID",
                table: "Specs");

            migrationBuilder.DropForeignKey(
                name: "FK_Specs_Users_EditUserID",
                table: "Specs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specs",
                table: "Specs");

            migrationBuilder.RenameTable(
                name: "Specs",
                newName: "Sepcs");

            migrationBuilder.RenameIndex(
                name: "IX_Specs_StatusID",
                table: "Sepcs",
                newName: "IX_Sepcs_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Specs_EditUserID",
                table: "Sepcs",
                newName: "IX_Sepcs_EditUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Specs_DeletedUserID",
                table: "Sepcs",
                newName: "IX_Sepcs_DeletedUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Specs_CreatedUserID",
                table: "Sepcs",
                newName: "IX_Sepcs_CreatedUserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sepcs",
                table: "Sepcs",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsSpecs_Sepcs_SpecID",
                table: "ProductsSpecs",
                column: "SpecID",
                principalTable: "Sepcs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sepcs_Statuses_StatusID",
                table: "Sepcs",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sepcs_Users_CreatedUserID",
                table: "Sepcs",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sepcs_Users_DeletedUserID",
                table: "Sepcs",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sepcs_Users_EditUserID",
                table: "Sepcs",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
