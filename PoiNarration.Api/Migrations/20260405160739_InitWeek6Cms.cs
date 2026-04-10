using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoiNarration.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitWeek6Cms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AudioUrlEn",
                table: "Booths",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AudioUrlVi",
                table: "Booths",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Booths",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MapUrl",
                table: "Booths",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TtsScriptEn",
                table: "Booths",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TtsScriptVi",
                table: "Booths",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Booth",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ZoneId = table.Column<string>(type: "TEXT", nullable: false),
                    NameVi = table.Column<string>(type: "TEXT", nullable: false),
                    NameEn = table.Column<string>(type: "TEXT", nullable: false),
                    DescVi = table.Column<string>(type: "TEXT", nullable: false),
                    DescEn = table.Column<string>(type: "TEXT", nullable: false),
                    Lat = table.Column<double>(type: "REAL", nullable: false),
                    Lng = table.Column<double>(type: "REAL", nullable: false),
                    RadiusMeters = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    OwnerUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booth", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booth");

            migrationBuilder.DropColumn(
                name: "AudioUrlEn",
                table: "Booths");

            migrationBuilder.DropColumn(
                name: "AudioUrlVi",
                table: "Booths");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Booths");

            migrationBuilder.DropColumn(
                name: "MapUrl",
                table: "Booths");

            migrationBuilder.DropColumn(
                name: "TtsScriptEn",
                table: "Booths");

            migrationBuilder.DropColumn(
                name: "TtsScriptVi",
                table: "Booths");
        }
    }
}
