using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoiNarration.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorAnalytics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VisitorUserId",
                table: "PlaybackLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrCodeUrl",
                table: "Booths",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "BoothMenuItemTranslations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "LocalizedPrice",
                table: "BoothMenuItemTranslations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceText",
                table: "BoothMenuItemTranslations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceUsd",
                table: "BoothMenuItems",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BoothVisitLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VisitorUserId = table.Column<string>(type: "TEXT", nullable: false),
                    BoothId = table.Column<string>(type: "TEXT", nullable: false),
                    TriggerType = table.Column<string>(type: "TEXT", nullable: false),
                    Language = table.Column<string>(type: "TEXT", nullable: false),
                    VisitedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", nullable: true),
                    Lat = table.Column<double>(type: "REAL", nullable: true),
                    Lng = table.Column<double>(type: "REAL", nullable: true),
                    IsSynced = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoothVisitLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitorUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    VisitorCode = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceKey = table.Column<string>(type: "TEXT", nullable: false),
                    PreferredLanguage = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", nullable: true),
                    AppVersion = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastActiveAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaybackLogs_VisitorUserId_PlayedAtUtc",
                table: "PlaybackLogs",
                columns: new[] { "VisitorUserId", "PlayedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_BoothVisitLogs_BoothId_VisitedAtUtc",
                table: "BoothVisitLogs",
                columns: new[] { "BoothId", "VisitedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_BoothVisitLogs_VisitorUserId_VisitedAtUtc",
                table: "BoothVisitLogs",
                columns: new[] { "VisitorUserId", "VisitedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_VisitorUsers_DeviceKey",
                table: "VisitorUsers",
                column: "DeviceKey");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorUsers_VisitorCode",
                table: "VisitorUsers",
                column: "VisitorCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoothVisitLogs");

            migrationBuilder.DropTable(
                name: "VisitorUsers");

            migrationBuilder.DropIndex(
                name: "IX_PlaybackLogs_VisitorUserId_PlayedAtUtc",
                table: "PlaybackLogs");

            migrationBuilder.DropColumn(
                name: "VisitorUserId",
                table: "PlaybackLogs");

            migrationBuilder.DropColumn(
                name: "QrCodeUrl",
                table: "Booths");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "BoothMenuItemTranslations");

            migrationBuilder.DropColumn(
                name: "LocalizedPrice",
                table: "BoothMenuItemTranslations");

            migrationBuilder.DropColumn(
                name: "PriceText",
                table: "BoothMenuItemTranslations");

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceUsd",
                table: "BoothMenuItems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }
    }
}
