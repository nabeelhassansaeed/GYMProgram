using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GYMProgram.Migrations
{
    public partial class update6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "DailyBookings");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "DailyBookings");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "DailyBookings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "DailyBookings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "DailyBookings");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "DailyBookings");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "DailyBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "DailyBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
