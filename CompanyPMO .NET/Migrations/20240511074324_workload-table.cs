using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class workloadtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "workload",
            //    table: "employees");

            migrationBuilder.CreateTable(
                name: "workload",
                columns: table => new
                {
                    workload_id = table.Column<int>(type: "integer", nullable: false),
                    workload_sum = table.Column<string>(type: "text", nullable: true),
                    assigned_projects = table.Column<int>(type: "integer", nullable: true),
                    completed_projects = table.Column<int>(type: "integer", nullable: true),
                    overdue_projects = table.Column<int>(type: "integer", nullable: true),
                    created_projects = table.Column<int>(type: "integer", nullable: true),
                    assigned_tasks = table.Column<int>(type: "integer", nullable: true),
                    completed_tasks = table.Column<int>(type: "integer", nullable: true),
                    overdue_tasks = table.Column<int>(type: "integer", nullable: true),
                    created_tasks = table.Column<int>(type: "integer", nullable: true),
                    assigned_issues = table.Column<int>(type: "integer", nullable: true),
                    completed_issues = table.Column<int>(type: "integer", nullable: true),
                    overdue_issues = table.Column<int>(type: "integer", nullable: true),
                    created_issues = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workload", x => x.workload_id);
                    table.ForeignKey(
                        name: "FK_workload_employees_workload_id",
                        column: x => x.workload_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 219, DateTimeKind.Utc).AddTicks(3228), "$2a$11$sfpyC32MytfkU3nPkbRwS.ZGiBkcW0nF6Ctb/Zr/T2CQOjWh7KIzy" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(5639), "$2a$11$I77JU/WCvj0jB3wsZZvEO.kjucIpmLK04QJ.rnQUXAVRXbFfZVO4i" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6135), new DateTime(2024, 5, 18, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6137), new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6136) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6139), new DateTime(2024, 5, 18, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6139), new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6139) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6274), new DateTime(2024, 5, 18, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6275), new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6274) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6276), new DateTime(2024, 5, 18, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6277), new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6277) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6279), new DateTime(2024, 5, 18, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6280), new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6279) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6054), new DateTime(2024, 6, 10, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6058), new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6068) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6070), new DateTime(2024, 7, 10, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6070), new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6074) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6075), new DateTime(2024, 8, 9, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6075), new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6076) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6095));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6097));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6098));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6100));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6102));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6103));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6104));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6105));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6108));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(521));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(522));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(192), new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(195) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(199), new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(199) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(201), new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(201) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "workload");

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
    }
}
