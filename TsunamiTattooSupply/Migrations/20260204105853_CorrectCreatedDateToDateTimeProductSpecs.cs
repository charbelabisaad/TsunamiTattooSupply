using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class CorrectCreatedDateToDateTimeProductSpecs : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
        ALTER TABLE ""ProductsSpecs""
        ALTER COLUMN ""CreatedDate""
        TYPE timestamp with time zone
        USING to_timestamp(""CreatedDate"");
    ");

			migrationBuilder.Sql(@"
        ALTER TABLE ""ProductsSpecs""
        ALTER COLUMN ""CreatedDate""
        SET DEFAULT now();
    ");
		}

	}
}
