using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AllProductRelationsInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductsColors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    ColorCode = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    ColorName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    IsCover = table.Column<bool>(type: "boolean", nullable: false),
                    StatusID = table.Column<string>(type: "character varying(1)", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsColors", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductsColors_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsColors_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsColors_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsColors_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProductsColors_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    StatusID = table.Column<string>(type: "character varying(1)", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Size_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Size_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Size_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Size_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ProductsImages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    ProductColorID = table.Column<int>(type: "integer", nullable: false),
                    SmallImage = table.Column<string>(type: "text", nullable: true),
                    OriginalImage = table.Column<string>(type: "text", nullable: true),
                    IsInitial = table.Column<bool>(type: "boolean", nullable: false),
                    StatusID = table.Column<string>(type: "character varying(1)", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductsImages_ProductsColors_ProductColorID",
                        column: x => x.ProductColorID,
                        principalTable: "ProductsColors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsImages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsImages_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsImages_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsImages_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProductsImages_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    SizeID = table.Column<int>(type: "integer", nullable: false),
                    CountryID = table.Column<int>(type: "integer", nullable: false),
                    CurrencyID = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    StatusID = table.Column<string>(type: "character varying(1)", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Prices_Countries_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Prices_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ProductsSizes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    SizeID = table.Column<int>(type: "integer", nullable: false),
                    Sale = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Raise = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    StatusID = table.Column<string>(type: "character varying(1)", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsSizes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductsSizes_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSizes_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSizes_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSizes_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSizes_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProductsSizes_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    SizeID = table.Column<int>(type: "integer", nullable: false),
                    ProductColorID = table.Column<int>(type: "integer", nullable: false),
                    Barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stocks_ProductsColors_ProductColorID",
                        column: x => x.ProductColorID,
                        principalTable: "ProductsColors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Stocks_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prices_CountryID",
                table: "Prices",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_CreatedUserID",
                table: "Prices",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_CurrencyID",
                table: "Prices",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_DeletedUserID",
                table: "Prices",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_EditUserID",
                table: "Prices",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ProductID",
                table: "Prices",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_SizeID",
                table: "Prices",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_StatusID",
                table: "Prices",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsColors_CreatedUserID",
                table: "ProductsColors",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsColors_DeletedUserID",
                table: "ProductsColors",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsColors_EditUserID",
                table: "ProductsColors",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsColors_ProductID",
                table: "ProductsColors",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsColors_StatusID",
                table: "ProductsColors",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsImages_CreatedUserID",
                table: "ProductsImages",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsImages_DeletedUserID",
                table: "ProductsImages",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsImages_EditUserID",
                table: "ProductsImages",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsImages_ProductColorID",
                table: "ProductsImages",
                column: "ProductColorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsImages_ProductID",
                table: "ProductsImages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsImages_StatusID",
                table: "ProductsImages",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_CreatedUserID",
                table: "ProductsSizes",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_DeletedUserID",
                table: "ProductsSizes",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_EditUserID",
                table: "ProductsSizes",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_ProductID",
                table: "ProductsSizes",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_SizeID",
                table: "ProductsSizes",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_StatusID",
                table: "ProductsSizes",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Size_CreatedUserID",
                table: "Size",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Size_DeletedUserID",
                table: "Size",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Size_Description",
                table: "Size",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Size_EditUserID",
                table: "Size",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Size_StatusID",
                table: "Size",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_CreatedUserID",
                table: "Stocks",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_DeletedUserID",
                table: "Stocks",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_EditUserID",
                table: "Stocks",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductColorID",
                table: "Stocks",
                column: "ProductColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductID",
                table: "Stocks",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_SizeID",
                table: "Stocks",
                column: "SizeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "ProductsImages");

            migrationBuilder.DropTable(
                name: "ProductsSizes");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "ProductsColors");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Brands");
        }
    }
}
