using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GYMProgram.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingsGuid",
                table: "DailyBookings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingsGuid",
                table: "DailyBookings",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
