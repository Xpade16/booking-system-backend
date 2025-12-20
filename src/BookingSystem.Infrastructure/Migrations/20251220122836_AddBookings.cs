using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClassScheduleId = table.Column<int>(type: "int", nullable: false),
                    UserPackageId = table.Column<int>(type: "int", nullable: false),
                    CreditsUsed = table.Column<int>(type: "int", nullable: false),
                    BookedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    CheckedInAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsCancelled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CancelledAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsRefunded = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Confirmed")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RowVersion = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_ClassSchedules_ClassScheduleId",
                        column: x => x.ClassScheduleId,
                        principalTable: "ClassSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_UserPackages_UserPackageId",
                        column: x => x.UserPackageId,
                        principalTable: "UserPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 21, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 21, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 21, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 21, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 22, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 22, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 22, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 22, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 23, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 23, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 23, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 23, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 24, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 24, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 24, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 24, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 25, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 25, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 25, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 25, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 26, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 26, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 26, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 27, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 27, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 27, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 28, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 28, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 28, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 28, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 29, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 29, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 29, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 29, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 30, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 30, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 30, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 30, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 31, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 12, 31, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2026, 1, 1, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2026, 1, 1, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2026, 1, 2, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 2, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2026, 1, 2, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 2, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2026, 1, 3, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 3, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627), new DateTime(2026, 1, 3, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 3, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 12, 28, 35, 419, DateTimeKind.Utc).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 12, 28, 35, 420, DateTimeKind.Utc).AddTicks(5134));

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 12, 28, 35, 420, DateTimeKind.Utc).AddTicks(5136));

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 12, 28, 35, 420, DateTimeKind.Utc).AddTicks(5139));

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 12, 28, 35, 420, DateTimeKind.Utc).AddTicks(5141));

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ClassScheduleId_Status",
                table: "Bookings",
                columns: new[] { "ClassScheduleId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId_BookedAt",
                table: "Bookings",
                columns: new[] { "UserId", "BookedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId_ClassScheduleId",
                table: "Bookings",
                columns: new[] { "UserId", "ClassScheduleId" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserPackageId",
                table: "Bookings",
                column: "UserPackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 18, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 18, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 18, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 19, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 19, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 19, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 19, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 20, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 20, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 20, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 21, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 21, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 21, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 21, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 22, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 22, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 22, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 22, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 23, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 23, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 23, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 23, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 24, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 24, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 24, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 24, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 25, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 25, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 25, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 25, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 26, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 26, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 26, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 27, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 27, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 27, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 28, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 28, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 28, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 28, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 29, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 29, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 29, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 29, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 30, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 30, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 30, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 30, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 31, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "CreatedAt", "EndTime", "StartTime", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), new DateTime(2025, 12, 31, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(8644));

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(8646));

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(8648));

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(8649));
        }
    }
}
