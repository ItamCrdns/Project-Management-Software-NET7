using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class updatedtaskprojectrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 634, DateTimeKind.Utc).AddTicks(8231), "$2a$11$QFvlogY81iugPkezv8/VO.x7Z9Kcy763WQYQ8Sn1LKyS8PMYO5vMG" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1040), "$2a$11$GKiLTvQj3dtXTPPUcNPqB.AGde.56HGGAXsTQoGzWE5wmn8cb6pEC" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1416), new DateTime(2024, 3, 25, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1417), new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1416) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1419), new DateTime(2024, 3, 25, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1420), new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1419) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1421), new DateTime(2024, 3, 25, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1422), new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1422) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1519), new DateTime(2024, 3, 25, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1519), new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1519) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1521), new DateTime(2024, 3, 25, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1522), new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1521) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1341), new DateTime(2024, 4, 17, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1342), new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1351) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1352), new DateTime(2024, 5, 17, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1353), new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1354) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1355), new DateTime(2024, 6, 16, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1356), new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1357) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1378));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1380));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1381));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1383));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1384));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1385));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1386));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1387));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 761, DateTimeKind.Utc).AddTicks(1388));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 508, DateTimeKind.Utc).AddTicks(8459));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 18, 16, 49, 58, 508, DateTimeKind.Utc).AddTicks(8460));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 508, DateTimeKind.Utc).AddTicks(7895), new DateTime(2024, 3, 18, 16, 49, 58, 508, DateTimeKind.Utc).AddTicks(7897) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 508, DateTimeKind.Utc).AddTicks(7901), new DateTime(2024, 3, 18, 16, 49, 58, 508, DateTimeKind.Utc).AddTicks(7902) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 18, 16, 49, 58, 508, DateTimeKind.Utc).AddTicks(7903), new DateTime(2024, 3, 18, 16, 49, 58, 508, DateTimeKind.Utc).AddTicks(7904) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 266, DateTimeKind.Utc).AddTicks(6378), "$2a$11$o376lQ0XWc1XCvDIVeegXuOET2pXbvurAceqpogdgyPbkltO7uRfG" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5176), "$2a$11$zA.naql9/aEcPOEYS2bPduLMFIScgwYJzgPvkKlXp6fBHWODBnXRu" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6220), new DateTime(2024, 3, 22, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6222), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6221) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6226), new DateTime(2024, 3, 22, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6228), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6227) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6230), new DateTime(2024, 3, 22, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6234), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6232) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6237), new DateTime(2024, 3, 22, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6240), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6238) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6242), new DateTime(2024, 3, 22, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6243), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6242) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5871), new DateTime(2024, 4, 14, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5873), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5890) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5894), new DateTime(2024, 5, 14, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5895), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5899) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5901), new DateTime(2024, 6, 13, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5902), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5903) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5948));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5952));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5954));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5956));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5959));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6166));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6168));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6170));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6173));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 88, DateTimeKind.Utc).AddTicks(7715));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 15, 0, 48, 22, 88, DateTimeKind.Utc).AddTicks(7719));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 86, DateTimeKind.Utc).AddTicks(4952), new DateTime(2024, 3, 15, 0, 48, 22, 86, DateTimeKind.Utc).AddTicks(4956) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 86, DateTimeKind.Utc).AddTicks(4962), new DateTime(2024, 3, 15, 0, 48, 22, 86, DateTimeKind.Utc).AddTicks(4962) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 86, DateTimeKind.Utc).AddTicks(4964), new DateTime(2024, 3, 15, 0, 48, 22, 86, DateTimeKind.Utc).AddTicks(4964) });
        }
    }
}
