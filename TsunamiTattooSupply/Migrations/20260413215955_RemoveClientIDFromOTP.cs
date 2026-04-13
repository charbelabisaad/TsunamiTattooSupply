using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
	public partial class RemoveClientIDFromOTP : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ClientID",
				table: "ClientsOTP"   // ✅ corrected
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "ClientID",
				table: "ClientsOTP",  // ✅ corrected
				type: "integer",
				nullable: true // adjust if needed
			);
		}
	}
}