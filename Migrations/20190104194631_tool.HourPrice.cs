using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vproker.Migrations
{
    public partial class toolHourPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HourPrice",
                table: "Tool",
                nullable: true,
                defaultValue: 0m);

            //migrationBuilder.AlterColumn<string>(
            //    name: "ClientPassport",
            //    table: "Order",
            //    nullable: true,
            //    oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourPrice",
                table: "Tool");

            //migrationBuilder.AlterColumn<string>(
            //    name: "ClientPassport",
            //    table: "Order",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);
        }
    }
}
