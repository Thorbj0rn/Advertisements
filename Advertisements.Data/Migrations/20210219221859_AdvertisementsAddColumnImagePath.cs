using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Advertisements.Data.Migrations
{
    public partial class AdvertisementsAddColumnImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Advertisements");
        }
    }
}
