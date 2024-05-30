using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class tieridfortimelinetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tier_id",
                table: "timelines",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_timelines_tier_id",
                table: "timelines",
                column: "tier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_timelines_tiers_tier_id",
                table: "timelines",
                column: "tier_id",
                principalTable: "tiers",
                principalColumn: "tier_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timelines_tiers_tier_id",
                table: "timelines");

            migrationBuilder.DropIndex(
                name: "IX_timelines_tier_id",
                table: "timelines");

            migrationBuilder.DropColumn(
                name: "tier_id",
                table: "timelines");
        }
    }
}
