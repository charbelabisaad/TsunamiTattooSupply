using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class CorrectProductSpecToDeletedUserID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsSpecs_Users_DeletedUserID",
                table: "ProductsSpecs");

            migrationBuilder.DropColumn(
                name: "DeleteUserID",
                table: "ProductsSpecs");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "ProductsSpecs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsSpecs_Users_DeletedUserID",
                table: "ProductsSpecs",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsSpecs_Users_DeletedUserID",
                table: "ProductsSpecs");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "ProductsSpecs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeleteUserID",
                table: "ProductsSpecs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsSpecs_Users_DeletedUserID",
                table: "ProductsSpecs",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
