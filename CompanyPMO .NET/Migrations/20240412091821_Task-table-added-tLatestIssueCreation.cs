using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class TasktableaddedtLatestIssueCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LatestIssueCreation",
                table: "tasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 617, DateTimeKind.Utc).AddTicks(8643), "$2a$11$zJ73nrD6ZO9j.3T4iSZrwOFPkzcNbMwoJoHWrZmVuP4pdfa0VwFTK" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9065), "$2a$11$PAjwX/fRSdSOV88Orym9Ku4e3jw74.6v1A80Lfiv.oXLw9AIAzSFO" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9580), new DateTime(2024, 4, 19, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9581), new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9580) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9583), new DateTime(2024, 4, 19, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9584), new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9583) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9585), new DateTime(2024, 4, 19, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9586), new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9586) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9588), new DateTime(2024, 4, 19, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9588), new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9588) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9690), new DateTime(2024, 4, 19, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9691), new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9691) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9509), new DateTime(2024, 5, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9510), new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9521) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9522), new DateTime(2024, 6, 11, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9523), new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9524) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9525), new DateTime(2024, 7, 11, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9526), new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9527) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                columns: new[] { "created", "LatestIssueCreation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9547), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                columns: new[] { "created", "LatestIssueCreation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9549), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                columns: new[] { "created", "LatestIssueCreation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9550), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                columns: new[] { "created", "LatestIssueCreation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9551), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                columns: new[] { "created", "LatestIssueCreation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9552), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                columns: new[] { "created", "LatestIssueCreation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9553), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                columns: new[] { "created", "LatestIssueCreation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9555), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                columns: new[] { "created", "LatestIssueCreation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9556), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                columns: new[] { "created", "LatestIssueCreation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9557), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 493, DateTimeKind.Utc).AddTicks(6200));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 493, DateTimeKind.Utc).AddTicks(6201));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 493, DateTimeKind.Utc).AddTicks(5933), new DateTime(2024, 4, 12, 9, 18, 21, 493, DateTimeKind.Utc).AddTicks(5935) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 493, DateTimeKind.Utc).AddTicks(5938), new DateTime(2024, 4, 12, 9, 18, 21, 493, DateTimeKind.Utc).AddTicks(5939) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 18, 21, 493, DateTimeKind.Utc).AddTicks(5940), new DateTime(2024, 4, 12, 9, 18, 21, 493, DateTimeKind.Utc).AddTicks(5941) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatestIssueCreation",
                table: "tasks");

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
    }
}
