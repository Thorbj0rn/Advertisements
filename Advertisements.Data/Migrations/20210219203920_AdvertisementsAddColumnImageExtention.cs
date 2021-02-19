using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Advertisements.Data.Migrations
{
    public partial class AdvertisementsAddColumnImageExtention : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7f57179d-466d-4bac-b84e-1f7360206d31"));

            migrationBuilder.RenameColumn(
                name: "DateCreate",
                table: "Advertisements",
                newName: "Created");

            migrationBuilder.AddColumn<string>(
                name: "ImageExtension",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Name", "PassKey", "Password", "Role" },
                values: new object[] { new Guid("2217bdbb-0288-4c12-bc65-9c8b854d27be"), "admin", "Admin", "aa78ac69-64b3-4142-98b5-f9ca1a5fae5d", "94-60-C9-7F-0F-2C-B5-BB-B7-3C-14-2F-12-E2-76-39-84-4C-47-23-E4-45-E5-BC-80-17-DA-F0-4E-DA-02-8E", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2217bdbb-0288-4c12-bc65-9c8b854d27be"));

            migrationBuilder.DropColumn(
                name: "ImageExtension",
                table: "Advertisements");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Advertisements",
                newName: "DateCreate");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Name", "PassKey", "Password", "Role" },
                values: new object[] { new Guid("7f57179d-466d-4bac-b84e-1f7360206d31"), "admin", "Admin", "aa78ac69-64b3-4142-98b5-f9ca1a5fae5dadmin", "7d8426e896cc7a056b09387014c42b170ac5e8cfdf8cebbbc493d1d1f7e0ed17", 1 });
        }
    }
}
