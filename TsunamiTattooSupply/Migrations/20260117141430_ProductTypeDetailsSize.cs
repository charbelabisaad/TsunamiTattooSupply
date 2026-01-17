using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class ProductTypeDetailsSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductDetailID",
                table: "ProductsSizes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductTypeID",
                table: "ProductsSizes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShowFront = table.Column<bool>(type: "boolean", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    StatusID = table.Column<string>(type: "character varying(1)", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: false),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProudctTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShowFront = table.Column<bool>(type: "boolean", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    StatusID = table.Column<string>(type: "character varying(1)", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: false),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProudctTypes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProudctTypes_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProudctTypes_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProudctTypes_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProudctTypes_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_ProductDetailID",
                table: "ProductsSizes",
                column: "ProductDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_ProductTypeID",
                table: "ProductsSizes",
                column: "ProductTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_CreatedUserID",
                table: "ProductDetails",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_DeletedUserID",
                table: "ProductDetails",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_EditUserID",
                table: "ProductDetails",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_StatusID",
                table: "ProductDetails",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ProudctTypes_CreatedUserID",
                table: "ProudctTypes",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProudctTypes_DeletedUserID",
                table: "ProudctTypes",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProudctTypes_EditUserID",
                table: "ProudctTypes",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProudctTypes_StatusID",
                table: "ProudctTypes",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsSizes_ProductDetails_ProductDetailID",
                table: "ProductsSizes",
                column: "ProductDetailID",
                principalTable: "ProductDetails",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsSizes_ProudctTypes_ProductTypeID",
                table: "ProductsSizes",
                column: "ProductTypeID",
                principalTable: "ProudctTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsSizes_ProductDetails_ProductDetailID",
                table: "ProductsSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsSizes_ProudctTypes_ProductTypeID",
                table: "ProductsSizes");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "ProudctTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductsSizes_ProductDetailID",
                table: "ProductsSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductsSizes_ProductTypeID",
                table: "ProductsSizes");

            migrationBuilder.DropColumn(
                name: "ProductDetailID",
                table: "ProductsSizes");

            migrationBuilder.DropColumn(
                name: "ProductTypeID",
                table: "ProductsSizes");
        }
    }
}
