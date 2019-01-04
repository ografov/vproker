using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vproker.Migrations
{
    public partial class extractclient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "ClientPhoneNumber",
            //    table: "Order",
            //    nullable: true,
            //    oldClrType: typeof(string));

            //migrationBuilder.AlterColumn<string>(
            //    name: "ClientName",
            //    table: "Order",
            //    nullable: true,
            //    oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ClientID",
                table: "Order",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Passport = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ClientID",
                table: "Order",
                column: "ClientID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Order_Clients_ClientID",
            //    table: "Order",
            //    column: "ClientID",
            //    principalTable: "Clients",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Order_Clients_ClientID",
            //    table: "Order");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Order_ClientID",
                table: "Order");

            //migrationBuilder.DropColumn(
            //    name: "ClientID",
            //    table: "Order");

            //migrationBuilder.AlterColumn<string>(
            //    name: "ClientPhoneNumber",
            //    table: "Order",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "ClientName",
            //    table: "Order",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);
        }
    }
}
