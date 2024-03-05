using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class addinglatestProjectCreationtocompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LatestProjectCreation",
                table: "companies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "companies",
                keyColumn: "company_id",
                keyValue: 1,
                column: "LatestProjectCreation",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "companies",
                keyColumn: "company_id",
                keyValue: 2,
                column: "LatestProjectCreation",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "companies",
                keyColumn: "company_id",
                keyValue: 3,
                column: "LatestProjectCreation",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 499, DateTimeKind.Utc).AddTicks(8510), "$2a$11$5X1CxvQmTmhomFIu0EqQ5.yoC9rdW.qka2ELHhsoqTgiL4XtcD8CG" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3364), "$2a$11$dgyHb9SVVsW59pE0qG53vO9dlOeFvxR67I0eoArjXlXOdxEXXuFxS" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4067), new DateTime(2024, 3, 12, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4068), new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4067) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4070), new DateTime(2024, 3, 12, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4071), new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4071) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4072), new DateTime(2024, 3, 12, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4073), new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4073) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4075), new DateTime(2024, 3, 12, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4075), new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4075) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4077), new DateTime(2024, 3, 12, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4078), new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4077) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3741), new DateTime(2024, 4, 4, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3743), new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3750) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3751), new DateTime(2024, 5, 4, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3752), new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3754) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3755), new DateTime(2024, 6, 3, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3755), new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3756) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3785));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3788));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3789));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3791));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3792));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3793));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(3794));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4033));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 625, DateTimeKind.Utc).AddTicks(4034));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 372, DateTimeKind.Utc).AddTicks(8958));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 5, 20, 1, 54, 372, DateTimeKind.Utc).AddTicks(8960));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 372, DateTimeKind.Utc).AddTicks(8512), new DateTime(2024, 3, 5, 20, 1, 54, 372, DateTimeKind.Utc).AddTicks(8514) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 372, DateTimeKind.Utc).AddTicks(8518), new DateTime(2024, 3, 5, 20, 1, 54, 372, DateTimeKind.Utc).AddTicks(8519) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 5, 20, 1, 54, 372, DateTimeKind.Utc).AddTicks(8520), new DateTime(2024, 3, 5, 20, 1, 54, 372, DateTimeKind.Utc).AddTicks(8520) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatestProjectCreation",
                table: "companies");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 787, DateTimeKind.Utc).AddTicks(5714), "$2a$11$wwfqaUQnEpnmOVDAx2sRY.wW5meZEYfjADufdlZKpG5IG2CjsTQIa" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4147), "$2a$11$t49UaYXbcrwHFuqd6Ru61ORES2wB3LXgXVvYB5Gst33gMZo60gbsG" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4638), new DateTime(2024, 2, 20, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4639), new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4638) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4641), new DateTime(2024, 2, 20, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4642), new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4641) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4643), new DateTime(2024, 2, 20, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4644), new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4643) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4645), new DateTime(2024, 2, 20, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4646), new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4646) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4647), new DateTime(2024, 2, 20, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4648), new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4648) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4561), new DateTime(2024, 3, 14, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4562), new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4575) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4576), new DateTime(2024, 4, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4577), new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4579) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4580), new DateTime(2024, 5, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4580), new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4581) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4602));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4603));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4605));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4606));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4607));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4608));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4609));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4611));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4612));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2725));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2726));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2356), new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2359) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2363), new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2363) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2365), new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2365) });
        }
    }
}
