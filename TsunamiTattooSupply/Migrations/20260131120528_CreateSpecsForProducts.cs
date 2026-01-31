using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class CreateSpecsForProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecsLabel",
                table: "SubCategories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Sepcs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusID = table.Column<string>(type: "character varying(1)", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sepcs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sepcs_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sepcs_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sepcs_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Sepcs_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ProductsSpecs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    SpecID = table.Column<int>(type: "integer", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<int>(type: "integer", nullable: false),
                    DeleteUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsSpecs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductsSpecs_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSpecs_Sepcs_SpecID",
                        column: x => x.SpecID,
                        principalTable: "Sepcs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSpecs_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSpecs_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSpecs_CreatedUserID",
                table: "ProductsSpecs",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSpecs_DeletedUserID",
                table: "ProductsSpecs",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSpecs_ProductID",
                table: "ProductsSpecs",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSpecs_SpecID",
                table: "ProductsSpecs",
                column: "SpecID");

            migrationBuilder.CreateIndex(
                name: "IX_Sepcs_CreatedUserID",
                table: "Sepcs",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Sepcs_DeletedUserID",
                table: "Sepcs",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Sepcs_EditUserID",
                table: "Sepcs",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Sepcs_StatusID",
                table: "Sepcs",
                column: "StatusID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsSpecs");

            migrationBuilder.DropTable(
                name: "Sepcs");

            migrationBuilder.DropColumn(
                name: "SpecsLabel",
                table: "SubCategories");
        }
    }
}
