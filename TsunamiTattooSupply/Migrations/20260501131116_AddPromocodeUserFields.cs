using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
	public partial class AddPromocodeUserFields : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// 🔥 User IDs (nullable, no default value)
			migrationBuilder.AddColumn<int?>(
				name: "CreatedUserID",
				table: "PromoCodes",
				nullable: true);

			migrationBuilder.AddColumn<int?>(
				name: "EditUserID",
				table: "PromoCodes",
				nullable: true);

			migrationBuilder.AddColumn<int?>(
				name: "DeletedUserID",
				table: "PromoCodes",
				nullable: true);

			// 🔥 Dates
			migrationBuilder.AddColumn<DateTime>(
				name: "CreationDate",
				table: "PromoCodes",
				type: "timestamp with time zone",
				nullable: false,
				defaultValueSql: "NOW()");

			migrationBuilder.AddColumn<DateTime>(
				name: "EditDate",
				table: "PromoCodes",
				type: "timestamp with time zone",
				nullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "DeletedDate",
				table: "PromoCodes",
				type: "timestamp with time zone",
				nullable: true);

			// 🔥 Foreign Keys
			migrationBuilder.AddForeignKey(
				name: "FK_PromoCodes_Users_CreatedUserID",
				table: "PromoCodes",
				column: "CreatedUserID",
				principalTable: "Users",
				principalColumn: "ID",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_PromoCodes_Users_EditUserID",
				table: "PromoCodes",
				column: "EditUserID",
				principalTable: "Users",
				principalColumn: "ID",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_PromoCodes_Users_DeletedUserID",
				table: "PromoCodes",
				column: "DeletedUserID",
				principalTable: "Users",
				principalColumn: "ID",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// 🔥 Drop Foreign Keys
			migrationBuilder.DropForeignKey(
				name: "FK_PromoCodes_Users_CreatedUserID",
				table: "PromoCodes");

			migrationBuilder.DropForeignKey(
				name: "FK_PromoCodes_Users_EditUserID",
				table: "PromoCodes");

			migrationBuilder.DropForeignKey(
				name: "FK_PromoCodes_Users_DeletedUserID",
				table: "PromoCodes");

			// 🔥 Drop Columns
			migrationBuilder.DropColumn(
				name: "CreatedUserID",
				table: "PromoCodes");

			migrationBuilder.DropColumn(
				name: "CreationDate",
				table: "PromoCodes");

			migrationBuilder.DropColumn(
				name: "EditUserID",
				table: "PromoCodes");

			migrationBuilder.DropColumn(
				name: "EditDate",
				table: "PromoCodes");

			migrationBuilder.DropColumn(
				name: "DeletedUserID",
				table: "PromoCodes");

			migrationBuilder.DropColumn(
				name: "DeletedDate",
				table: "PromoCodes");
		}
	}
}