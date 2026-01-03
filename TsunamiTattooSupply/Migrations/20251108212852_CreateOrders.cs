using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TsunamiTattooSupply.Migrations
{
    /// <inheritdoc />
    public partial class CreateOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrackingOrderStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "char(3)", nullable: false),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingOrderStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClientID = table.Column<int>(type: "integer", nullable: false),
                    ClientAddressID = table.Column<int>(type: "integer", nullable: false),
                    PromoCode = table.Column<string>(type: "text", nullable: false),
                    PromoCodePercentage = table.Column<int>(type: "integer", nullable: false),
                    Tax = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    MoneyTransferFees = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    CurrencyID = table.Column<int>(type: "integer", nullable: false),
                    VAT = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    VATAmount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    GobalDiscount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    CurrentTrackingOrderStatusID = table.Column<int>(type: "integer", nullable: false),
                    CurrentTrackingOrderDate = table.Column<DateTime>(type: "date", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_ClientAddresses_ClientAddressID",
                        column: x => x.ClientAddressID,
                        principalTable: "ClientAddresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_TrackingOrderStatuses_CurrentTrackingOrderStatusID",
                        column: x => x.CurrentTrackingOrderStatusID,
                        principalTable: "TrackingOrderStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Orders_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderID = table.Column<int>(type: "integer", nullable: false),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    SizeID = table.Column<int>(type: "integer", nullable: false),
                    ColorID = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    QuantityPending = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    QuantityDelivered = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    QuantityCancelled = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    VATAmount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    PriceNet = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    CancellationReason = table.Column<string>(type: "text", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditUserID = table.Column<int>(type: "integer", nullable: true),
                    EditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedUserID = table.Column<int>(type: "integer", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Colors_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Users_DeletedUserID",
                        column: x => x.DeletedUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_OrderProducts_Users_EditUserID",
                        column: x => x.EditUserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TrackingOrders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderID = table.Column<int>(type: "integer", nullable: false),
                    TrackingOrderStatusID = table.Column<int>(type: "integer", nullable: false),
                    CreatedUserID = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingOrders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TrackingOrders_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackingOrders_TrackingOrderStatuses_TrackingOrderStatusID",
                        column: x => x.TrackingOrderStatusID,
                        principalTable: "TrackingOrderStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackingOrders_Users_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ColorID",
                table: "OrderProducts",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_DeletedUserID",
                table: "OrderProducts",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_EditUserID",
                table: "OrderProducts",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderID",
                table: "OrderProducts",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductID",
                table: "OrderProducts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_SizeID",
                table: "OrderProducts",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientAddressID",
                table: "Orders",
                column: "ClientAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientID",
                table: "Orders",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Code",
                table: "Orders",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CurrencyID",
                table: "Orders",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CurrentTrackingOrderStatusID",
                table: "Orders",
                column: "CurrentTrackingOrderStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeletedUserID",
                table: "Orders",
                column: "DeletedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EditUserID",
                table: "Orders",
                column: "EditUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentCode",
                table: "Orders",
                column: "PaymentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingOrders_CreatedUserID",
                table: "TrackingOrders",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingOrders_OrderID",
                table: "TrackingOrders",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingOrders_TrackingOrderStatusID",
                table: "TrackingOrders",
                column: "TrackingOrderStatusID");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "TrackingOrders");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "TrackingOrderStatuses");
        }
    }
}
