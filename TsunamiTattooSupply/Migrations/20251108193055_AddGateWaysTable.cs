using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddGateWaysTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GetWays",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MerchantID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MerchantPassword = table.Column<string>(type: "text", nullable: false),
                    Mode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Url = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    StatusID = table.Column<string>(type: "character varying(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetWays", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GetWays_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GetWays_StatusID",
                table: "GetWays",
                column: "StatusID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetWays");
        }
    }
}
