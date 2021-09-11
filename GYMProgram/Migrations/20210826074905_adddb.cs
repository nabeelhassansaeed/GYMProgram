using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GYMProgram.Migrations
{
    public partial class adddb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GYMs",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    LatinName = table.Column<string>(maxLength: 150, nullable: true),
                    Location = table.Column<string>(maxLength: 250, nullable: true),
                    Notes = table.Column<string>(maxLength: 800, nullable: true),
                    Logo = table.Column<string>(maxLength: 250, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GYMs", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    LatinName = table.Column<string>(maxLength: 150, nullable: true),
                    Notes = table.Column<string>(maxLength: 800, nullable: true),
                    ExpirDate = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    GYMGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Customer_GYM_GYMGuid",
                        column: x => x.GYMGuid,
                        principalTable: "GYMs",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    LatinName = table.Column<string>(maxLength: 150, nullable: true),
                    Notes = table.Column<string>(maxLength: 800, nullable: true),
                    GYMGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Section_GYM_GYMGuid",
                        column: x => x.GYMGuid,
                        principalTable: "GYMs",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Number = table.Column<int>(nullable: false),
                    SectionGuid = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    QTYCustomers = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Bookings_Section_SectionGuid",
                        column: x => x.SectionGuid,
                        principalTable: "Sections",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnavailableDates",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Date = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(maxLength: 800, nullable: true),
                    SectionGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnavailableDates", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UnavailableDate_Section_SectionGuid",
                        column: x => x.SectionGuid,
                        principalTable: "Sections",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeekDaysHeads",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    SectionGuid = table.Column<Guid>(nullable: false),
                    Notes = table.Column<string>(maxLength: 800, nullable: false),
                    Number = table.Column<int>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekDaysHeads", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_WeekDaysHeads_Sections_SectionGuid",
                        column: x => x.SectionGuid,
                        principalTable: "Sections",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyBookings",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    CUGuid = table.Column<Guid>(nullable: true),
                    BookingGuid = table.Column<Guid>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<bool>(nullable: false, defaultValue: false),
                    Number = table.Column<int>(nullable: false, defaultValue: 0),
                    BookingsGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyBookings", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_DailyBooking_Booking_DayGuid",
                        column: x => x.BookingGuid,
                        principalTable: "Bookings",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_DailyBooking_Customer_CUGuid",
                        column: x => x.CUGuid,
                        principalTable: "Customers",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeekDays",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    DayNumber = table.Column<int>(nullable: false),
                    DayName = table.Column<string>(maxLength: 20, nullable: true),
                    DayLatinName = table.Column<string>(maxLength: 20, nullable: true),
                    Notes = table.Column<string>(maxLength: 800, nullable: true),
                    WorkStartHour = table.Column<DateTime>(nullable: false),
                    WorkEndHour = table.Column<DateTime>(nullable: false),
                    Minutes = table.Column<string>(maxLength: 20, nullable: true),
                    HeadGuid = table.Column<Guid>(nullable: true),
                    QTYCustomers = table.Column<int>(nullable: false, defaultValue: 1),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekDays", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_WeekDay_WeekDaysHead_HeadGuid",
                        column: x => x.HeadGuid,
                        principalTable: "WeekDaysHeads",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SectionGuid",
                table: "Bookings",
                column: "SectionGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_GYMGuid",
                table: "Customers",
                column: "GYMGuid");

            migrationBuilder.CreateIndex(
                name: "IX_DailyBookings_BookingGuid",
                table: "DailyBookings",
                column: "BookingGuid");

            migrationBuilder.CreateIndex(
                name: "IX_DailyBookings_CUGuid",
                table: "DailyBookings",
                column: "CUGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_GYMGuid",
                table: "Sections",
                column: "GYMGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UnavailableDates_SectionGuid",
                table: "UnavailableDates",
                column: "SectionGuid");

            migrationBuilder.CreateIndex(
                name: "IX_WeekDays_HeadGuid",
                table: "WeekDays",
                column: "HeadGuid");

            migrationBuilder.CreateIndex(
                name: "IX_WeekDaysHeads_SectionGuid",
                table: "WeekDaysHeads",
                column: "SectionGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DailyBookings");

            migrationBuilder.DropTable(
                name: "UnavailableDates");

            migrationBuilder.DropTable(
                name: "WeekDays");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "WeekDaysHeads");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "GYMs");
        }
    }
}
