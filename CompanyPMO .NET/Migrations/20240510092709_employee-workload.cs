using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class employeeworkload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "workload",
                table: "employees",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password", "workload" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 410, DateTimeKind.Utc).AddTicks(8461), "$2a$11$zJbUnUzRZEV/GPHd1Vfwf.fJgW3YrFYDF.Jy5leo/ecqqiWhsMiWS", null });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password", "workload" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(5555), "$2a$11$fXAmj0o50YYkpYclWtUtF.WXUDX9sM5rVCLyDN5UiCo6kHwFU8u4e", null });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6154), new DateTime(2024, 5, 17, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6155), new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6155) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6158), new DateTime(2024, 5, 17, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6159), new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6158) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6160), new DateTime(2024, 5, 17, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6161), new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6161) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6163), new DateTime(2024, 5, 17, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6164), new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6163) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6165), new DateTime(2024, 5, 17, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6166), new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6165) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(5981), new DateTime(2024, 6, 9, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(5982), new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(5990) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(5992), new DateTime(2024, 7, 9, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(5992), new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(5993) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(5994), new DateTime(2024, 8, 8, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(5995), new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(5996) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6017));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6019));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6020));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6022));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6023));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6024));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6125));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6127));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 537, DateTimeKind.Utc).AddTicks(6128));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 283, DateTimeKind.Utc).AddTicks(2147));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 5, 10, 9, 27, 9, 283, DateTimeKind.Utc).AddTicks(2148));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 283, DateTimeKind.Utc).AddTicks(1759), new DateTime(2024, 5, 10, 9, 27, 9, 283, DateTimeKind.Utc).AddTicks(1762) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 283, DateTimeKind.Utc).AddTicks(1765), new DateTime(2024, 5, 10, 9, 27, 9, 283, DateTimeKind.Utc).AddTicks(1765) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 5, 10, 9, 27, 9, 283, DateTimeKind.Utc).AddTicks(1766), new DateTime(2024, 5, 10, 9, 27, 9, 283, DateTimeKind.Utc).AddTicks(1767) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "workload",
                table: "employees");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 550, DateTimeKind.Utc).AddTicks(688), "$2a$11$R5afQKMZ2Xj19XP1aoge7OAP8jAbjUpAFI98XsM.kdYAX1m1VQfEq" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(6703), "$2a$11$sywVmuoAjHuiCqnUZMfiGuLdWVIQxgoGTMIWANUTtR29u.4gnRNrK" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8335), new DateTime(2024, 5, 1, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8337), new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8336) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8340), new DateTime(2024, 5, 1, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8341), new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8341) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8343), new DateTime(2024, 5, 1, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8346), new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8343) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8347), new DateTime(2024, 5, 1, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8348), new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8348) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8350), new DateTime(2024, 5, 1, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8351), new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8350) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8158), new DateTime(2024, 5, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8161), new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8177) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8179), new DateTime(2024, 6, 23, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8179), new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8183) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8185), new DateTime(2024, 7, 23, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8185), new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8186) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8276));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8278));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8280));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8281));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8283));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8285));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8288));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8289));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 799, DateTimeKind.Utc).AddTicks(8291));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 408, DateTimeKind.Utc).AddTicks(2230));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 11, 46, 408, DateTimeKind.Utc).AddTicks(2232));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 408, DateTimeKind.Utc).AddTicks(1638), new DateTime(2024, 4, 24, 3, 11, 46, 408, DateTimeKind.Utc).AddTicks(1642) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 408, DateTimeKind.Utc).AddTicks(1648), new DateTime(2024, 4, 24, 3, 11, 46, 408, DateTimeKind.Utc).AddTicks(1649) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 11, 46, 408, DateTimeKind.Utc).AddTicks(1650), new DateTime(2024, 4, 24, 3, 11, 46, 408, DateTimeKind.Utc).AddTicks(1651) });
        }
    }
}
