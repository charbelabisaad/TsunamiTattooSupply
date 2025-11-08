using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class UnitCurrencyProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Users_DeletedUserID",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Users_EditUserID",
                table: "Brands");

            migrationBuilder.AlterColumn<int>(
                name: "EditUserID",
                table: "Brands",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "Brands",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Brands",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Brands",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditDate",
                table: "Brands",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Priority = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
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
                    table.PrimaryKey("PK_Currencies", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Currencies_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Currencies_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Currencies_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Currencies_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShortDescription = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LongDescription = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_Units", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Units_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Units_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Units_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Units_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CurrenciesConversion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrencyIDFrom = table.Column<int>(type: "integer", maxLength: 3, nullable: false),
                    CurrencyIDTo = table.Column<int>(type: "integer", maxLength: 3, nullable: false),
                    Operator = table.Column<int>(type: "integer", maxLength: 3, nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrenciesConversion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CurrenciesConversion_Currencies_CurrencyIDFrom",
                        column: x => x.CurrencyIDFrom,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrenciesConversion_Currencies_CurrencyIDTo",
                        column: x => x.CurrencyIDTo,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrenciesConversion_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UnitID = table.Column<int>(type: "integer", nullable: false),
                    BrandID = table.Column<int>(type: "integer", nullable: false),
                    VAT = table.Column<bool>(type: "boolean", nullable: false),
                    Feature = table.Column<bool>(type: "boolean", nullable: false),
                    NewArrival = table.Column<bool>(type: "boolean", nullable: false),
                    NewArrivalDateExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    Warranty = table.Column<bool>(type: "boolean", nullable: false),
                    WarrantyMonths = table.Column<int>(type: "integer", nullable: false),
                    VideoUrl = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Units_UnitID",
                        column: x => x.UnitID,
                        principalTable: "Units",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Products_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ProductsSubCategories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubCategoryID = table.Column<int>(type: "integer", nullable: false),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ProductsSubCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductsSubCategories_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSubCategories_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSubCategories_SubCategories_SubCategoryID",
                        column: x => x.SubCategoryID,
                        principalTable: "SubCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSubCategories_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsSubCategories_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ProductsSubCategories_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                table: "Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CreatedUserID",
                table: "Currencies",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_DeletedUserID",
                table: "Currencies",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Description",
                table: "Currencies",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_EditUserID",
                table: "Currencies",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_StatusID",
                table: "Currencies",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesConversion_CurrencyIDFrom",
                table: "CurrenciesConversion",
                column: "CurrencyIDFrom");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesConversion_CurrencyIDTo",
                table: "CurrenciesConversion",
                column: "CurrencyIDTo");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesConversion_EditUserID",
                table: "CurrenciesConversion",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandID",
                table: "Products",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedUserID",
                table: "Products",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DeletedUserID",
                table: "Products",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_EditUserID",
                table: "Products",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_StatusID",
                table: "Products",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitID",
                table: "Products",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSubCategories_CreatedUserID",
                table: "ProductsSubCategories",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSubCategories_DeletedUserID",
                table: "ProductsSubCategories",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSubCategories_EditUserID",
                table: "ProductsSubCategories",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSubCategories_ProductID",
                table: "ProductsSubCategories",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSubCategories_StatusID",
                table: "ProductsSubCategories",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSubCategories_SubCategoryID",
                table: "ProductsSubCategories",
                column: "SubCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Units_CreatedUserID",
                table: "Units",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Units_DeletedUserID",
                table: "Units",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Units_EditUserID",
                table: "Units",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Units_LongDescription",
                table: "Units",
                column: "LongDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_ShortDescription",
                table: "Units",
                column: "ShortDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_StatusID",
                table: "Units",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Users_DeletedUserID",
                table: "Brands",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Users_EditUserID",
                table: "Brands",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Users_DeletedUserID",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Users_EditUserID",
                table: "Brands");

            migrationBuilder.DropTable(
                name: "CurrenciesConversion");

            migrationBuilder.DropTable(
                name: "ProductsSubCategories");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "EditDate",
                table: "Brands");

            migrationBuilder.AlterColumn<int>(
                name: "EditUserID",
                table: "Brands",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedUserID",
                table: "Brands",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Users_DeletedUserID",
                table: "Brands",
                column: "DeletedUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Users_EditUserID",
                table: "Brands",
                column: "EditUserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
