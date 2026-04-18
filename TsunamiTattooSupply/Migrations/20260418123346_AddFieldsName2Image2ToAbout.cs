using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsName2Image2ToAbout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortText",
                table: "Abouts",
                newName: "ShortText2");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Abouts",
                newName: "Image2");

            migrationBuilder.AddColumn<string>(
                name: "Image1",
                table: "Abouts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortText1",
                table: "Abouts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "ShortText1",
                table: "Abouts");

            migrationBuilder.RenameColumn(
                name: "ShortText2",
                table: "Abouts",
                newName: "ShortText");

            migrationBuilder.RenameColumn(
                name: "Image2",
                table: "Abouts",
                newName: "Image");
        }
    }
}
