using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vproker.Migrations
{
    public partial class clientcreatedAtdescriptiondob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Clients",
                nullable: true);
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Clients",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Clients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Clients");
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Clients");
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Clients");
        }
    }
}
