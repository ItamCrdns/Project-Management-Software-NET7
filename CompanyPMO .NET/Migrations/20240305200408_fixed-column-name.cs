using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class fixedcolumnname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LatestProjectCreation",
                table: "companies",
                newName: "latest_project_creation");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "latest_project_creation",
                table: "companies",
                newName: "LatestProjectCreation");

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
    }
}
