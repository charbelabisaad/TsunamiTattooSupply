using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeIDToColorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropColumn(
				name: "IsCustom",
				table: "Colors");

			migrationBuilder.AddColumn<int>(
				name: "TypeID",
				table: "Colors",
				type: "integer",
				nullable: false,
				defaultValue: 1);

			migrationBuilder.CreateIndex(
				name: "IX_Colors_TypeID",
				table: "Colors",
				column: "TypeID");

			migrationBuilder.AddForeignKey(
				name: "FK_Colors_ColorTypes_TypeID",
				table: "Colors",
				column: "TypeID",
				principalTable: "ColorTypes",
				principalColumn: "ID",
				onDelete: ReferentialAction.Cascade);

		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropForeignKey(
				name: "FK_Colors_ColorTypes_TypeID",
				table: "Colors");

			migrationBuilder.DropIndex(
				name: "IX_Colors_TypeID",
				table: "Colors");

			migrationBuilder.DropColumn(
				name: "TypeID",
				table: "Colors");

			migrationBuilder.AddColumn<bool>(
				name: "IsCustom",
				table: "Colors",
				type: "boolean",
				nullable: false,
				defaultValue: false);

		}
	}
}
