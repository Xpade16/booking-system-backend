using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClassSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    AvailableSlots = table.Column<int>(type: "int", nullable: false),
                    RequiredCredits = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    RowVersion = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSchedules_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ClassSchedules",
                columns: new[] { "Id", "AvailableSlots", "Capacity", "CountryId", "CreatedAt", "Description", "EndTime", "IsActive", "RequiredCredits", "StartTime", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 18, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 1", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 2, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 18, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 18, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 1", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 3, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 19, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 19, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 2", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 4, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 19, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 19, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 2", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 5, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 20, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 20, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 3", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 6, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 20, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 20, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 3", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 7, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 21, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 21, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 4", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 8, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 21, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 21, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 4", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 9, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 22, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 22, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 5", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 10, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 22, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 22, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 5", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 11, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 23, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 23, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 6", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 12, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 23, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 23, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 6", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 13, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 24, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 24, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 7", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 14, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 24, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 24, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 7", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 15, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 25, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 25, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 8", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 16, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 25, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 25, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 8", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 17, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 26, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 26, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 9", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 18, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 26, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 26, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 9", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 19, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 27, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 10", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 20, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 27, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 27, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 10", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 21, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 28, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 28, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 11", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 22, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 28, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 28, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 11", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 23, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 29, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 29, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 12", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 24, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 29, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 29, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 12", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 25, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 30, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 30, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 13", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 26, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 30, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 30, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 13", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 27, 15, 15, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "Start your day with energizing yoga poses and breathing exercises. Suitable for all levels.", new DateTime(2025, 12, 31, 11, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 31, 10, 0, 0, 0, DateTimeKind.Utc), "Morning Yoga - Day 14", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) },
                    { 28, 20, 20, 1, new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352), "High-intensity interval training to boost your metabolism and build strength. Intermediate level.", new DateTime(2025, 12, 31, 19, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2025, 12, 31, 18, 0, 0, 0, DateTimeKind.Utc), "Evening HIIT - Day 14", new DateTime(2025, 12, 17, 10, 26, 31, 6, DateTimeKind.Utc).AddTicks(1352) }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_CountryId_StartTime_IsActive",
                table: "ClassSchedules",
                columns: new[] { "CountryId", "StartTime", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_StartTime",
                table: "ClassSchedules",
                column: "StartTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassSchedules");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 13, 18, 44, 53, 647, DateTimeKind.Utc).AddTicks(7737));

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 13, 18, 44, 53, 647, DateTimeKind.Utc).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 13, 18, 44, 53, 647, DateTimeKind.Utc).AddTicks(7740));

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 13, 18, 44, 53, 647, DateTimeKind.Utc).AddTicks(7742));
        }
    }
}
