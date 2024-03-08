using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class employeeresetpasswordtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reset_password_token",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "reset_password_token_expiry",
                table: "employees");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 308, DateTimeKind.Utc).AddTicks(7488), "$2a$11$Gmtf6OhL0oD8heQz4AQcE.HbEhbZWtGERH3uf9q3HPYXhzctRpmYG" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(4575), "$2a$11$TW6e2YPWTqVxf/7PZgriBetbXxxUn3tNrtCNtoivSUzaEcKO/wqVW" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5217), new DateTime(2024, 3, 12, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5218), new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5217) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5220), new DateTime(2024, 3, 12, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5221), new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5220) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5222), new DateTime(2024, 3, 12, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5223), new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5222) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5224), new DateTime(2024, 3, 12, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5225), new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5225) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5226), new DateTime(2024, 3, 12, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5227), new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5227) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(4986), new DateTime(2024, 4, 4, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(4987), new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(4993) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(4995), new DateTime(2024, 5, 4, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(4996), new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(4997) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(4998), new DateTime(2024, 6, 3, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(4999), new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5000) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5024));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5026));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5027));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5029));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5030));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5031));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5032));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5033));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 438, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 181, DateTimeKind.Utc).AddTicks(6533));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 4, 8, 181, DateTimeKind.Utc).AddTicks(6534));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 181, DateTimeKind.Utc).AddTicks(6134), new DateTime(2024, 3, 5, 20, 4, 8, 181, DateTimeKind.Utc).AddTicks(6136) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 181, DateTimeKind.Utc).AddTicks(6140), new DateTime(2024, 3, 5, 20, 4, 8, 181, DateTimeKind.Utc).AddTicks(6140) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 4, 8, 181, DateTimeKind.Utc).AddTicks(6141), new DateTime(2024, 3, 5, 20, 4, 8, 181, DateTimeKind.Utc).AddTicks(6142) });
        }
    }
}
