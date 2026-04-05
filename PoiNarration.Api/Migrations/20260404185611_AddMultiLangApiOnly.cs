using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoiNarration.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiLangApiOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "BoothMenuItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "BoothMenuItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceUsd",
                table: "BoothMenuItems",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "BoothMenuItems");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "BoothMenuItems");

            migrationBuilder.DropColumn(
                name: "PriceUsd",
                table: "BoothMenuItems");
        }
    }
}
