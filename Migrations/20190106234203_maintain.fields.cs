using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vproker.Migrations
{
    public partial class maintainfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EngineHours",
                table: "Maintains",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Materials",
                table: "Maintains",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Maintains",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ToolID",
                table: "Maintains",
                nullable: true,
                defaultValue: "");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "CreatedAt",
            //    table: "Clients",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "DateOfBirth",
            //    table: "Clients",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Description",
            //    table: "Clients",
            //    nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maintains_ToolID",
                table: "Maintains",
                column: "ToolID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Maintains_Tool_ToolID",
            //    table: "Maintains",
            //    column: "ToolID",
            //    principalTable: "Tool",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Maintains_Tool_ToolID",
            //    table: "Maintains");

            migrationBuilder.DropIndex(
                name: "IX_Maintains_ToolID",
                table: "Maintains");

            migrationBuilder.DropColumn(
                name: "EngineHours",
                table: "Maintains");

            migrationBuilder.DropColumn(
                name: "Materials",
                table: "Maintains");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Maintains");

            migrationBuilder.DropColumn(
                name: "ToolID",
                table: "Maintains");

            //migrationBuilder.DropColumn(
            //    name: "CreatedAt",
            //    table: "Clients");

            //migrationBuilder.DropColumn(
            //    name: "DateOfBirth",
            //    table: "Clients");

            //migrationBuilder.DropColumn(
            //    name: "Description",
            //    table: "Clients");

            //migrationBuilder.AlterColumn<decimal>(
            //    name: "HourPrice",
            //    table: "Tool",
            //    nullable: false,
            //    oldClrType: typeof(decimal),
            //    oldNullable: true);
        }
    }
}
