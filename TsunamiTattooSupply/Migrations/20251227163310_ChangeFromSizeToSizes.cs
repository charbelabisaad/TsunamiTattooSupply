using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFromSizeToSizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Size_SizeID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Size_SizeID",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Size_SizeID",
                table: "Prices");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsBestSeller_Size_SizeID",
                table: "ProductsBestSeller");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsSizes_Size_SizeID",
                table: "ProductsSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Size_Statuses_StatusID",
                table: "Size");

            migrationBuilder.DropForeignKey(
                name: "FK_Size_Users_CreatedUserID",
                table: "Size");

            migrationBuilder.DropForeignKey(
                name: "FK_Size_Users_DeletedUserID",
                table: "Size");

            migrationBuilder.DropForeignKey(
                name: "FK_Size_Users_EditUserID",
                table: "Size");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Size_SizeID",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Size",
                table: "Size");

            migrationBuilder.RenameTable(
                name: "Size",
                newName: "Sizes");

            migrationBuilder.RenameIndex(
                name: "IX_Size_StatusID",
                table: "Sizes",
                newName: "IX_Sizes_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Size_EditUserID",
                table: "Sizes",
                newName: "IX_Sizes_EditUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Size_DeletedUserID",
                table: "Sizes",
                newName: "IX_Sizes_DeletedUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Size_CreatedUserID",
                table: "Sizes",
                newName: "IX_Sizes_CreatedUserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Sizes_SizeID",
                table: "Carts",
                column: "SizeID",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Sizes_SizeID",
                table: "OrderProducts",
                column: "SizeID",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Sizes_SizeID",
                table: "Prices",
                column: "SizeID",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsBestSeller_Sizes_SizeID",
                table: "ProductsBestSeller",
                column: "SizeID",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsSizes_Sizes_SizeID",
                table: "ProductsSizes",
                column: "SizeID",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Statuses_StatusID",
                table: "Sizes",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Users_CreatedUserID",
                table: "Sizes",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Users_DeletedUserID",
                table: "Sizes",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Users_EditUserID",
                table: "Sizes",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Sizes_SizeID",
                table: "Stocks",
                column: "SizeID",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Sizes_SizeID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Sizes_SizeID",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Sizes_SizeID",
                table: "Prices");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsBestSeller_Sizes_SizeID",
                table: "ProductsBestSeller");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsSizes_Sizes_SizeID",
                table: "ProductsSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Statuses_StatusID",
                table: "Sizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Users_CreatedUserID",
                table: "Sizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Users_DeletedUserID",
                table: "Sizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Users_EditUserID",
                table: "Sizes");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Sizes_SizeID",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sizes",
                table: "Sizes");

            migrationBuilder.RenameTable(
                name: "Sizes",
                newName: "Size");

            migrationBuilder.RenameIndex(
                name: "IX_Sizes_StatusID",
                table: "Size",
                newName: "IX_Size_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_Sizes_EditUserID",
                table: "Size",
                newName: "IX_Size_EditUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Sizes_DeletedUserID",
                table: "Size",
                newName: "IX_Size_DeletedUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Sizes_CreatedUserID",
                table: "Size",
                newName: "IX_Size_CreatedUserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Size",
                table: "Size",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Size_SizeID",
                table: "Carts",
                column: "SizeID",
                principalTable: "Size",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Size_SizeID",
                table: "OrderProducts",
                column: "SizeID",
                principalTable: "Size",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Size_SizeID",
                table: "Prices",
                column: "SizeID",
                principalTable: "Size",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsBestSeller_Size_SizeID",
                table: "ProductsBestSeller",
                column: "SizeID",
                principalTable: "Size",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsSizes_Size_SizeID",
                table: "ProductsSizes",
                column: "SizeID",
                principalTable: "Size",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Size_Statuses_StatusID",
                table: "Size",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Size_Users_CreatedUserID",
                table: "Size",
                column: "CreatedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Size_Users_DeletedUserID",
                table: "Size",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Size_Users_EditUserID",
                table: "Size",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Size_SizeID",
                table: "Stocks",
                column: "SizeID",
                principalTable: "Size",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
