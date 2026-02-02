using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class CorrectSpecDeletedDateFromIntToDateTime : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("""
        ALTER TABLE "Sepcs"
        ALTER COLUMN "DeletedDate"
        TYPE timestamp with time zone
        USING to_timestamp("DeletedDate");
    """);

			migrationBuilder.AlterColumn<decimal>(
				name: "TotalNet",
				table: "Orders",
				type: "numeric(12,2)",
				nullable: true,
				oldClrType: typeof(decimal),
				oldType: "decimal(12,2)",
				oldNullable: true);

			migrationBuilder.AlterColumn<decimal>(
				name: "TotalFixed",
				table: "Orders",
				type: "numeric(12,2)",
				nullable: true,
				oldClrType: typeof(decimal),
				oldType: "decimal(12,2)",
				oldNullable: true);
		}


		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DeletedDate",
                table: "Sepcs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalNet",
                table: "Orders",
                type: "decimat(12,2",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalFixed",
                table: "Orders",
                type: "decimal(12,2",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,2)",
                oldNullable: true);
        }
    }
}
