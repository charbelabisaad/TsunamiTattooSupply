using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BannerImage = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Image = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AD_Image1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AD_Image2 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AD_Image3 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AD_Details = table.Column<string>(type: "text", nullable: true),
                    MobileImage = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Categories_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Categories_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedUserID",
                table: "Categories",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DeletedUserID",
                table: "Categories",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Description",
                table: "Categories",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_EditUserID",
                table: "Categories",
                column: "EditUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
