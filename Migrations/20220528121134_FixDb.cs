using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace vproker.Migrations
{
    public partial class FixDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql( "UPDATE [Clients] SET PhoneNumber = '99999999999' WHERE PhoneNumber IS ''" );
            migrationBuilder.Sql( "UPDATE [Clients] SET DateOfBirth = '0001-01-01 00:00:00' WHERE DateOfBirth IS NULL" );
            migrationBuilder.Sql( "UPDATE [Order] SET ContractNumber = '0' WHERE ContractNumber is null" );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
