using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddBooleanPlcedConfirmedReceivedToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.AddColumn<bool>(
                name: "IsPlaced",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReceived",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.DropColumn(
                name: "IsPlaced",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsReceived",
                table: "Orders");
        }
    }
}
