using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class TasktableaddedtLatestIssueCreationfixedtypoincolumnname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LatestIssueCreation",
                table: "tasks",
                newName: "latest_issue_creation");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 31, 7, DateTimeKind.Utc).AddTicks(5329), "$2a$11$2aU1/jzbN1VwifOINSbXjOBM9sFRTPlRp6CfeeGRCPvWpWcDj6uT6" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6055), "$2a$11$8X9NUjw43tCeNAq9NzlePe/Bt4BaiafSyuP/V.ShRdtUW1RKM2Aia" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6490), new DateTime(2024, 4, 19, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6491), new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6490) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6493), new DateTime(2024, 4, 19, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6494), new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6493) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6495), new DateTime(2024, 4, 19, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6496), new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6496) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6498), new DateTime(2024, 4, 19, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6498), new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6498) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6500), new DateTime(2024, 4, 19, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6501), new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6500) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6334), new DateTime(2024, 5, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6335), new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6342) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6344), new DateTime(2024, 6, 11, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6344), new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6345) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6346), new DateTime(2024, 7, 11, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6347), new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6348) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6373));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6375));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6376));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6377));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6378));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6379));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6380));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6465));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 31, 132, DateTimeKind.Utc).AddTicks(6466));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 30, 883, DateTimeKind.Utc).AddTicks(2634));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 19, 30, 883, DateTimeKind.Utc).AddTicks(2636));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 30, 883, DateTimeKind.Utc).AddTicks(2234), new DateTime(2024, 4, 12, 9, 19, 30, 883, DateTimeKind.Utc).AddTicks(2236) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 30, 883, DateTimeKind.Utc).AddTicks(2240), new DateTime(2024, 4, 12, 9, 19, 30, 883, DateTimeKind.Utc).AddTicks(2240) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 12, 9, 19, 30, 883, DateTimeKind.Utc).AddTicks(2242), new DateTime(2024, 4, 12, 9, 19, 30, 883, DateTimeKind.Utc).AddTicks(2242) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "latest_issue_creation",
                table: "tasks",
                newName: "LatestIssueCreation");

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
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9547));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9549));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9550));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9551));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9552));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9553));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9555));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9556));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 4, 12, 9, 18, 21, 744, DateTimeKind.Utc).AddTicks(9557));

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
    }
}
