using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace vproker.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KnowledgeSource",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeSource", x => x.ID);
                });
            migrationBuilder.CreateTable(
                name: "Tool",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    DayPrice = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Pledge = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    WorkShiftPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tool", x => x.ID);
                });
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    DiscountPercent = table.Column<int>(nullable: false),
                    DocumentData = table.Column<byte[]>(nullable: true),
                    DocumentGivenBy = table.Column<string>(nullable: false),
                    DocumentGivenWhen = table.Column<DateTime>(nullable: false),
                    DocumentNumber = table.Column<string>(nullable: false),
                    DocumentSerial = table.Column<string>(nullable: false),
                    DocumentUnitCode = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    KnowSourceID = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    LivingAddress = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Phone1 = table.Column<string>(nullable: true),
                    Phone2 = table.Column<string>(nullable: true),
                    Phone3 = table.Column<string>(nullable: true),
                    RegistrationAddress = table.Column<string>(nullable: true),
                    WorkingAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Client_KnowledgeSource_KnowSourceID",
                        column: x => x.KnowSourceID,
                        principalTable: "KnowledgeSource",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    ClientName = table.Column<string>(nullable: false),
                    ClientPhoneNumber = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    PaidPledge = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    ToolID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Tool_ToolID",
                        column: x => x.ToolID,
                        principalTable: "Tool",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Client");
            migrationBuilder.DropTable("Order");
            migrationBuilder.DropTable("KnowledgeSource");
            migrationBuilder.DropTable("Tool");
        }
    }
}
