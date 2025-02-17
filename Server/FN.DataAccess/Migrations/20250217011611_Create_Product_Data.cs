using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FN.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Create_Product_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeCreated",
                table: "app_users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 17, 8, 16, 10, 605, DateTimeKind.Local).AddTicks(8730),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 16, 2, 24, 30, 659, DateTimeKind.Local).AddTicks(2503));

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoTitle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoAlias = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoKeyword = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Other = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizeName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoTitle = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoAlias = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoDescription = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SeoKeyword = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Thumbnail = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 606, DateTimeKind.Utc).AddTicks(3300)),
                    ModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 606, DateTimeKind.Utc).AddTicks(3779))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_items_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_items_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Detail = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LikeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DisLikeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Version = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_products_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product_images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PublicId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UrlImage = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Alt = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_images_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product_prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    PriceType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 607, DateTimeKind.Utc).AddTicks(9345)),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 607, DateTimeKind.Utc).AddTicks(9735)),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2025, 2, 17, 1, 16, 10, 608, DateTimeKind.Utc).AddTicks(119))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_prices_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_items_CategoryId",
                table: "items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_items_Code",
                table: "items",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_items_SeoAlias",
                table: "items",
                column: "SeoAlias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_items_UserId",
                table: "items",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_product_images_ProductId",
                table: "product_images",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_images_PublicId",
                table: "product_images",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_prices_ProductId",
                table: "product_prices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_products_ItemId",
                table: "products",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_images");

            migrationBuilder.DropTable(
                name: "product_prices");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeCreated",
                table: "app_users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 16, 2, 24, 30, 659, DateTimeKind.Local).AddTicks(2503),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2025, 2, 17, 8, 16, 10, 605, DateTimeKind.Local).AddTicks(8730));
        }
    }
}
