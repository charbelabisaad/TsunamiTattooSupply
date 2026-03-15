using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPageLocationToBannerPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "BannersPages",
                newName: "AppType");

            migrationBuilder.AddColumn<int>(
                name: "PageLocationID",
                table: "BannersPages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PageLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageLocations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_PageLocationID",
                table: "BannersPages",
                column: "PageLocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_BannersPages_PageLocations_PageLocationID",
                table: "BannersPages",
                column: "PageLocationID",
                principalTable: "PageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannersPages_PageLocations_PageLocationID",
                table: "BannersPages");

            migrationBuilder.DropTable(
                name: "PageLocations");

            migrationBuilder.DropIndex(
                name: "IX_BannersPages_PageLocationID",
                table: "BannersPages");

            migrationBuilder.DropColumn(
                name: "PageLocationID",
                table: "BannersPages");

            migrationBuilder.RenameColumn(
                name: "AppType",
                table: "BannersPages",
                newName: "Type");
        }
    }
}
