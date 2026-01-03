using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class CoutriesAndClientsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ISO2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ISO3 = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShippingCost = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    ShippingCostFixed = table.Column<bool>(type: "boolean", nullable: false),
                    Sales = table.Column<bool>(type: "boolean", nullable: false),
                    TotalRange = table.Column<double>(type: "double precision", nullable: false),
                    MoneyTransferFees = table.Column<double>(type: "double precision", nullable: false),
                    IP = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Native = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PasswordMobile = table.Column<string>(type: "char(10)", nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PhoneCountryID = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DeviceToken = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    VerificationCode = table.Column<string>(type: "text", nullable: true),
                    ResetPasswordLink = table.Column<string>(type: "text", nullable: true),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Clients_Countries_PhoneCountryID",
                        column: x => x.PhoneCountryID,
                        principalTable: "Countries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clients_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clients_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Clients_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CreatedUserID",
                table: "Clients",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_DeletedUserID",
                table: "Clients",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_EditUserID",
                table: "Clients",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PhoneCountryID",
                table: "Clients",
                column: "PhoneCountryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
