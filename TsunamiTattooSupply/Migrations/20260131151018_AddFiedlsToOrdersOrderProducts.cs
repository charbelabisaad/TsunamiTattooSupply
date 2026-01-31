using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddFiedlsToOrdersOrderProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalFixed",
                table: "Orders",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalNet",
                table: "Orders",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductDetailID",
                table: "OrderProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductTypeID",
                table: "OrderProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalFixed",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalNet",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductDetailID",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ProductTypeID",
                table: "OrderProducts");
        }
    }
}
