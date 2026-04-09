using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFieldAboutTable : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("""
                ALTER TABLE "Abouts"
                ALTER COLUMN "ID" DROP IDENTITY IF EXISTS;
            """);

			migrationBuilder.AlterColumn<string>(
				name: "ID",
				table: "Abouts",
				type: "text",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer");

			migrationBuilder.AddColumn<string>(
				name: "Image",
				table: "Abouts",
				type: "text",
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Image",
				table: "Abouts");

			migrationBuilder.AlterColumn<int>(
				name: "ID",
				table: "Abouts",
				type: "integer",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "text")
				.Annotation("Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
		}
	}
}
