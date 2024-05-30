using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class missingnotificationnavigationproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_notifications_receiver_id",
                table: "notifications",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_sender_id",
                table: "notifications",
                column: "sender_id");

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_employees_receiver_id",
                table: "notifications",
                column: "receiver_id",
                principalTable: "employees",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_employees_sender_id",
                table: "notifications",
                column: "sender_id",
                principalTable: "employees",
                principalColumn: "employee_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notifications_employees_receiver_id",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_notifications_employees_sender_id",
                table: "notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_receiver_id",
                table: "notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_sender_id",
                table: "notifications");
        }
    }
}
