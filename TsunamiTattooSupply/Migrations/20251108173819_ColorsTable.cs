using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class ColorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorID",
                table: "ProductsColors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    StatusID = table.Column<string>(type: "character varying(1)", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: false),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Colors_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Colors_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Colors_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Colors_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsColors_ColorID",
                table: "ProductsColors",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_Code",
                table: "Colors",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_CreatedUserID",
                table: "Colors",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_DeletedUserID",
                table: "Colors",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_EditUserID",
                table: "Colors",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_Name",
                table: "Colors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_StatusID",
                table: "Colors",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsColors_Colors_ColorID",
                table: "ProductsColors",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsColors_Colors_ColorID",
                table: "ProductsColors");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_ProductsColors_ColorID",
                table: "ProductsColors");

            migrationBuilder.DropColumn(
                name: "ColorID",
                table: "ProductsColors");
        }
    }
}
