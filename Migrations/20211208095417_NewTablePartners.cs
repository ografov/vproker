using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vproker.Migrations
{
    public partial class NewTablePartners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Discount = table.Column<float>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partners");
        }
    }
}
