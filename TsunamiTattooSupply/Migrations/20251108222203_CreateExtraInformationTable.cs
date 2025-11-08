using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class CreateExtraInformationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "POSStocks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POSStocks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProductsBestSeller",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    SizeID = table.Column<int>(type: "integer", nullable: false),
                    ColorID = table.Column<int>(type: "integer", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsBestSeller", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductsBestSeller_Colors_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsBestSeller_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsBestSeller_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsBestSeller_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsRecentlyViewed",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientID = table.Column<int>(type: "integer", nullable: false),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsRecentlyViewed", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductsRecentlyViewed_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsRecentlyViewed_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsWishList",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientID = table.Column<int>(type: "integer", nullable: false),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsWishList", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductsWishList_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsWishList_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Services_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SocialMedias",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedias", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_POSStocks_Barcode",
                table: "POSStocks",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsBestSeller_ColorID",
                table: "ProductsBestSeller",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsBestSeller_CreatedUserID",
                table: "ProductsBestSeller",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsBestSeller_ProductID",
                table: "ProductsBestSeller",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsBestSeller_SizeID",
                table: "ProductsBestSeller",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsRecentlyViewed_ClientID",
                table: "ProductsRecentlyViewed",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsRecentlyViewed_ProductID",
                table: "ProductsRecentlyViewed",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsWishList_ClientID",
                table: "ProductsWishList",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsWishList_ProductID",
                table: "ProductsWishList",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CreatedUserID",
                table: "Services",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_EditUserID",
                table: "Services",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMedias_Code",
                table: "SocialMedias",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialMedias_Description",
                table: "SocialMedias",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Email",
                table: "Subscriptions",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POSStocks");

            migrationBuilder.DropTable(
                name: "ProductsBestSeller");

            migrationBuilder.DropTable(
                name: "ProductsRecentlyViewed");

            migrationBuilder.DropTable(
                name: "ProductsWishList");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "SocialMedias");

            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
