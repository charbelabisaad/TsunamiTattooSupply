using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteForeignKetContraintStatusID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_TrackingOrderStatuses_CurrentTrackingOrderStatusID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CurrentTrackingOrderStatusID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CurrentTrackingOrderStatusID",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentTrackingOrderStatusID",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CurrentTrackingOrderStatusID",
                table: "Orders",
                column: "CurrentTrackingOrderStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_TrackingOrderStatuses_CurrentTrackingOrderStatusID",
                table: "Orders",
                column: "CurrentTrackingOrderStatusID",
                principalTable: "TrackingOrderStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
