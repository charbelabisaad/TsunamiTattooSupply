using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AllBannersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    Sentence = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Banners", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Banners_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Banners_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Banners_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Banners_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "BannersMobiles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(type: "text", nullable: false),
                    CategoryID = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_BannersMobiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BannersMobiles_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannersMobiles_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannersMobiles_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannersMobiles_SubCategories_SubCategoryID",
                        column: x => x.SubCategoryID,
                        principalTable: "SubCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannersMobiles_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannersMobiles_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_BannersMobiles_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "BannersPages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_BannersPages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BannersPages_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannersPages_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannersPages_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_BannersPages_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banners_CreatedUserID",
                table: "Banners",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_DeletedUserID",
                table: "Banners",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_EditUserID",
                table: "Banners",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_StatusID",
                table: "Banners",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersMobiles_CategoryID",
                table: "BannersMobiles",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersMobiles_CreatedUserID",
                table: "BannersMobiles",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersMobiles_DeletedUserID",
                table: "BannersMobiles",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersMobiles_EditUserID",
                table: "BannersMobiles",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersMobiles_ProductID",
                table: "BannersMobiles",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersMobiles_StatusID",
                table: "BannersMobiles",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersMobiles_SubCategoryID",
                table: "BannersMobiles",
                column: "SubCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_Code",
                table: "BannersPages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_CreatedUserID",
                table: "BannersPages",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_DeletedUserID",
                table: "BannersPages",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_EditUserID",
                table: "BannersPages",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_Name",
                table: "BannersPages",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_StatusID",
                table: "BannersPages",
                column: "StatusID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "BannersMobiles");

            migrationBuilder.DropTable(
                name: "BannersPages");
        }
    }
}
