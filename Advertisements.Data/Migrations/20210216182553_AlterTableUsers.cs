using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Advertisements.Data.Migrations
{
    public partial class AlterTableUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassKey",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Name", "PassKey", "Password", "Role" },
                values: new object[] { new Guid("7f57179d-466d-4bac-b84e-1f7360206d31"), "admin", "Admin", "aa78ac69-64b3-4142-98b5-f9ca1a5fae5dadmin", "7d8426e896cc7a056b09387014c42b170ac5e8cfdf8cebbbc493d1d1f7e0ed17", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7f57179d-466d-4bac-b84e-1f7360206d31"));

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PassKey",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");
        }
    }
}
