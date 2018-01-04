using Microsoft.EntityFrameworkCore.Migrations;

namespace vproker.Migrations
{
    public partial class contractnumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractNumber",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ContractNumber", table: "Order");
        }
    }
}
