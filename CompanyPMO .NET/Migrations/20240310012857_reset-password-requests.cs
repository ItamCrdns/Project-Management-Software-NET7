using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class resetpasswordrequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reset_password_token",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "reset_password_token_expiry",
                table: "employees");

            migrationBuilder.CreateTable(
                name: "ResetPasswordRequests",
                columns: table => new
                {
                    request_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    token = table.Column<int>(type: "integer", nullable: true),
                    token_expiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPasswordRequests", x => x.request_id);
                    table.ForeignKey(
                        name: "FK_ResetPasswordRequests_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 112, DateTimeKind.Utc).AddTicks(7188), "$2a$11$Nu.DryNSLwbQsTPUKeYAYuuutEMUp16kNdd8roY1IgYzoh6V/6lbO" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5372), "$2a$11$tGb8YWdzjaOhhJ7c422h5OnppcVWr9T9P7JZk6vPmHZkEq0NSrPG2" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5848), new DateTime(2024, 3, 17, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5850), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5849) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5852), new DateTime(2024, 3, 17, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5854), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5853) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5856), new DateTime(2024, 3, 17, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5857), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5856) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5859), new DateTime(2024, 3, 17, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5860), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5859) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5862), new DateTime(2024, 3, 17, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5863), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5862) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5737), new DateTime(2024, 4, 9, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5742), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5753) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5756), new DateTime(2024, 5, 9, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5756), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5759) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5760), new DateTime(2024, 6, 8, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5761), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5762) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5796));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5798));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5800));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5802));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5803));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5805));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5809));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5811));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5814));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(1537));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(1538));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(964), new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(965) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(969), new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(970) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(971), new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(972) });

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswordRequests_employee_id",
                table: "ResetPasswordRequests",
                column: "employee_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResetPasswordRequests");

            migrationBuilder.AddColumn<int>(
                name: "reset_password_token",
                table: "employees",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "reset_password_token_expiry",
                table: "employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password", "reset_password_token", "reset_password_token_expiry" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 303, DateTimeKind.Utc).AddTicks(5943), "$2a$11$gsJn3WfYiZHnFaPcoT8VAOyblykoksIrWIfY5Yj5Qc8feK0tQekmu", null, null });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password", "reset_password_token", "reset_password_token_expiry" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(2825), "$2a$11$BN5kuSQl6NfSq.4VT6955ejRCgJ1TT0uCLTYjq9ddpxBmeDTQrRVO", null, null });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3222), new DateTime(2024, 3, 15, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3223), new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3222) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3225), new DateTime(2024, 3, 15, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3226), new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3225) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3227), new DateTime(2024, 3, 15, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3228), new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3228) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3229), new DateTime(2024, 3, 15, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3230), new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3230) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3232), new DateTime(2024, 3, 15, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3232), new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3232) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3149), new DateTime(2024, 4, 7, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3150), new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3161) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3163), new DateTime(2024, 5, 7, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3163), new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3164) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3165), new DateTime(2024, 6, 6, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3166), new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3167) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3186));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3188));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3189));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3190));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3191));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3193));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3194));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3195));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 432, DateTimeKind.Utc).AddTicks(3196));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 174, DateTimeKind.Utc).AddTicks(7509));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 8, 13, 44, 25, 174, DateTimeKind.Utc).AddTicks(7510));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 174, DateTimeKind.Utc).AddTicks(7067), new DateTime(2024, 3, 8, 13, 44, 25, 174, DateTimeKind.Utc).AddTicks(7069) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 174, DateTimeKind.Utc).AddTicks(7073), new DateTime(2024, 3, 8, 13, 44, 25, 174, DateTimeKind.Utc).AddTicks(7073) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 8, 13, 44, 25, 174, DateTimeKind.Utc).AddTicks(7075), new DateTime(2024, 3, 8, 13, 44, 25, 174, DateTimeKind.Utc).AddTicks(7075) });
        }
    }
}
