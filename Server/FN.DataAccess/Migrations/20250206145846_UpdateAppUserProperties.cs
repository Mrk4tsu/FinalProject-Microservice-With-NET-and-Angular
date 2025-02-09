using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FN.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppUserProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "app_users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "https://res.cloudinary.com/dje3seaqj/image/upload/v1736989161/gatapchoi_biglrl.jpg")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreated",
                table: "app_users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 6, 21, 58, 45, 806, DateTimeKind.Local).AddTicks(5288));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "app_users");

            migrationBuilder.DropColumn(
                name: "TimeCreated",
                table: "app_users");
        }
    }
}
