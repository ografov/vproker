using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vproker.Migrations
{
    public partial class ClientPassport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //    table: "AspNetRoleClaims");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //    table: "AspNetUserClaims");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //    table: "AspNetUserLogins");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //    table: "AspNetUserRoles");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //    table: "AspNetUserRoles");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_vproker.Models.Order_vproker.Models.Tool_ToolID",
            //    table: "vproker.Models.Order");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_vproker.Models.Tool",
            //    table: "vproker.Models.Tool");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_vproker.Models.Order",
            //    table: "vproker.Models.Order");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            //migrationBuilder.RenameTable(
            //    name: "vproker.Models.Tool",
            //    newName: "Tool");

            //migrationBuilder.RenameTable(
            //    name: "vproker.Models.Order",
            //    newName: "Order");

            //migrationBuilder.AlterColumn<string>(
            //    name: "ContractNumber",
            //    table: "Order",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientPassport",
                table: "Order",
                nullable: true,
                defaultValue: "");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Tool",
            //    table: "Tool",
            //    column: "ID");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Order",
            //    table: "Order",
            //    column: "ID");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ToolID",
                table: "Order",
                column: "ToolID");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //    table: "AspNetRoleClaims",
            //    column: "RoleId",
            //    principalTable: "AspNetRoles",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //    table: "AspNetUserClaims",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //    table: "AspNetUserLogins",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //    table: "AspNetUserRoles",
            //    column: "RoleId",
            //    principalTable: "AspNetRoles",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //    table: "AspNetUserRoles",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Order_Tool_ToolID",
            //    table: "Order",
            //    column: "ToolID",
            //    principalTable: "Tool",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //    table: "AspNetRoleClaims");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //    table: "AspNetUserClaims");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //    table: "AspNetUserLogins");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //    table: "AspNetUserRoles");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //    table: "AspNetUserRoles");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Order_Tool_ToolID",
            //    table: "Order");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Tool",
            //    table: "Tool");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Order",
            //    table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ToolID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "ClientPassport",
                table: "Order");

            //migrationBuilder.RenameTable(
            //    name: "Tool",
            //    newName: "vproker.Models.Tool");

            //migrationBuilder.RenameTable(
            //    name: "Order",
            //    newName: "vproker.Models.Order");

            //migrationBuilder.AlterColumn<string>(
            //    name: "ContractNumber",
            //    table: "vproker.Models.Order",
            //    nullable: true,
            //    oldClrType: typeof(string));

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_vproker.Models.Tool",
            //    table: "vproker.Models.Tool",
            //    column: "ID");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_vproker.Models.Order",
            //    table: "vproker.Models.Order",
            //    column: "ID");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //    table: "AspNetRoleClaims",
            //    column: "RoleId",
            //    principalTable: "AspNetRoles",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //    table: "AspNetUserClaims",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //    table: "AspNetUserLogins",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //    table: "AspNetUserRoles",
            //    column: "RoleId",
            //    principalTable: "AspNetRoles",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //    table: "AspNetUserRoles",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_vproker.Models.Order_vproker.Models.Tool_ToolID",
            //    table: "vproker.Models.Order",
            //    column: "ToolID",
            //    principalTable: "vproker.Models.Tool",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
