using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FN.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Update_Price : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_product_details_ProductDetailId",
                table: "ProductPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices");

            migrationBuilder.RenameTable(
                name: "ProductPrices",
                newName: "product_prices");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPrices_ProductDetailId",
                table: "product_prices",
                newName: "IX_product_prices_ProductDetailId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "items",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 20, 0, 4, 6, 275, DateTimeKind.Local).AddTicks(6878),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 20, 0, 2, 53, 63, DateTimeKind.Local).AddTicks(2422));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "items",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 20, 0, 4, 6, 275, DateTimeKind.Local).AddTicks(6269),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 20, 0, 2, 53, 63, DateTimeKind.Local).AddTicks(1941));

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeCreated",
                table: "app_users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 20, 0, 4, 6, 274, DateTimeKind.Local).AddTicks(8231),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 20, 0, 2, 53, 62, DateTimeKind.Local).AddTicks(5758));

            migrationBuilder.AlterColumn<int>(
                name: "PriceType",
                table: "product_prices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "product_prices",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "product_prices",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 19, 17, 4, 6, 277, DateTimeKind.Utc).AddTicks(1303),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_prices",
                table: "product_prices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_product_prices_product_details_ProductDetailId",
                table: "product_prices",
                column: "ProductDetailId",
                principalTable: "product_details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_prices_product_details_ProductDetailId",
                table: "product_prices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_prices",
                table: "product_prices");

            migrationBuilder.RenameTable(
                name: "product_prices",
                newName: "ProductPrices");

            migrationBuilder.RenameIndex(
                name: "IX_product_prices_ProductDetailId",
                table: "ProductPrices",
                newName: "IX_ProductPrices_ProductDetailId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "items",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 20, 0, 2, 53, 63, DateTimeKind.Local).AddTicks(2422),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 20, 0, 4, 6, 275, DateTimeKind.Local).AddTicks(6878));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "items",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 20, 0, 2, 53, 63, DateTimeKind.Local).AddTicks(1941),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 20, 0, 4, 6, 275, DateTimeKind.Local).AddTicks(6269));

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeCreated",
                table: "app_users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 20, 0, 2, 53, 62, DateTimeKind.Local).AddTicks(5758),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 20, 0, 4, 6, 274, DateTimeKind.Local).AddTicks(8231));

            migrationBuilder.AlterColumn<int>(
                name: "PriceType",
                table: "ProductPrices",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ProductPrices",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductPrices",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 19, 17, 4, 6, 277, DateTimeKind.Utc).AddTicks(1303));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_product_details_ProductDetailId",
                table: "ProductPrices",
                column: "ProductDetailId",
                principalTable: "product_details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
