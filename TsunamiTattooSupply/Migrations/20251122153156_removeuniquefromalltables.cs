using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class removeuniquefromalltables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Units_LongDescription",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_ShortDescription",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_TrackingOrderStatuses_Code",
                table: "TrackingOrderStatuses");

            migrationBuilder.DropIndex(
                name: "IX_TrackingOrderStatuses_Description",
                table: "TrackingOrderStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_Email",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_Description",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Size_Description",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Description",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Products_Code",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_POSStocks_Barcode",
                table: "POSStocks");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_Code",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_Description",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Code",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentCode",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_Code",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_Description",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Name",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Colors_Code",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Colors_Name",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Description",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Brands_Name",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_BannersPages_Code",
                table: "BannersPages");

            migrationBuilder.DropIndex(
                name: "IX_BannersPages_Name",
                table: "BannersPages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_LongDescription",
                table: "Units",
                column: "LongDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_ShortDescription",
                table: "Units",
                column: "ShortDescription",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingOrderStatuses_Code",
                table: "TrackingOrderStatuses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingOrderStatuses_Description",
                table: "TrackingOrderStatuses",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Email",
                table: "Subscriptions",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_Description",
                table: "SubCategories",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Size_Description",
                table: "Size",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Description",
                table: "Roles",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_POSStocks_Barcode",
                table: "POSStocks",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Code",
                table: "Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Description",
                table: "Permissions",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Code",
                table: "Orders",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentCode",
                table: "Orders",
                column: "PaymentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                table: "Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Description",
                table: "Currencies",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_Code",
                table: "Colors",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_Name",
                table: "Colors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Description",
                table: "Categories",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_Code",
                table: "BannersPages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannersPages_Name",
                table: "BannersPages",
                column: "Name",
                unique: true);
        }
    }
}
