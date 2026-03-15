using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToBannerPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "BannersPages",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "BannersPages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasPeriod",
                table: "BannersPages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Present",
                table: "BannersPages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "BannersPages",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "BannersPages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryID",
                table: "BannersPages",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "BannersPages",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "BannersPages");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BannersPages");

            migrationBuilder.DropColumn(
                name: "HasPeriod",
                table: "BannersPages");

            migrationBuilder.DropColumn(
                name: "Present",
                table: "BannersPages");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "BannersPages");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "BannersPages");

            migrationBuilder.DropColumn(
                name: "SubCategoryID",
                table: "BannersPages");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "BannersPages");
        }
    }
}
