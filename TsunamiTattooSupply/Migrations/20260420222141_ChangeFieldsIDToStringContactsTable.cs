using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFieldsIDToStringContactsTable : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("""
                ALTER TABLE "Contacts"
                ALTER COLUMN "ID" DROP IDENTITY IF EXISTS;
            """);

			        migrationBuilder.AlterColumn<string>(
				        name: "ID",
				        table: "Contacts",
				        type: "character varying(10)",
				        maxLength: 10,
				        nullable: false,
				        oldClrType: typeof(int),
				        oldType: "integer");
		        }

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Contacts",
                type: "integer",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
