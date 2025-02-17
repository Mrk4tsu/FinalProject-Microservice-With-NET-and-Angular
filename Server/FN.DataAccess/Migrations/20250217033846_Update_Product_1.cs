using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FN.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Update_Product_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DownloadCount",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "product_prices",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 3, 38, 46, 16, DateTimeKind.Utc).AddTicks(2655),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 607, DateTimeKind.Utc).AddTicks(9345));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "product_prices",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 3, 38, 46, 16, DateTimeKind.Utc).AddTicks(3163),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 607, DateTimeKind.Utc).AddTicks(9735));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "product_prices",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 3, 38, 46, 16, DateTimeKind.Utc).AddTicks(3581),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 608, DateTimeKind.Utc).AddTicks(119));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "items",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 3, 38, 46, 14, DateTimeKind.Utc).AddTicks(5183),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 606, DateTimeKind.Utc).AddTicks(3779));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "items",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 3, 38, 46, 14, DateTimeKind.Utc).AddTicks(4658),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 606, DateTimeKind.Utc).AddTicks(3300));

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeCreated",
                table: "app_users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 10, 38, 46, 13, DateTimeKind.Local).AddTicks(7575),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 8, 16, 10, 605, DateTimeKind.Local).AddTicks(8730));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "product_prices",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 607, DateTimeKind.Utc).AddTicks(9345),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 3, 38, 46, 16, DateTimeKind.Utc).AddTicks(2655));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "product_prices",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 607, DateTimeKind.Utc).AddTicks(9735),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 3, 38, 46, 16, DateTimeKind.Utc).AddTicks(3163));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "product_prices",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 608, DateTimeKind.Utc).AddTicks(119),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 3, 38, 46, 16, DateTimeKind.Utc).AddTicks(3581));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "items",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 606, DateTimeKind.Utc).AddTicks(3779),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 3, 38, 46, 14, DateTimeKind.Utc).AddTicks(5183));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "items",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 606, DateTimeKind.Utc).AddTicks(3300),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 3, 38, 46, 14, DateTimeKind.Utc).AddTicks(4658));

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeCreated",
                table: "app_users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 8, 16, 10, 605, DateTimeKind.Local).AddTicks(8730),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 10, 38, 46, 13, DateTimeKind.Local).AddTicks(7575));
        }
    }
}
