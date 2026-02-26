using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class SetColorIDRequiredInPriceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Colors_ColorID",
                table: "Prices");

            migrationBuilder.AlterColumn<int>(
                name: "ColorID",
                table: "Prices",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Colors_ColorID",
                table: "Prices",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Colors_ColorID",
                table: "Prices");

            migrationBuilder.AlterColumn<int>(
                name: "ColorID",
                table: "Prices",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Colors_ColorID",
                table: "Prices",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "ID");
        }
    }
}
