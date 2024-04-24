using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class Fixedemployeecolumnname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordVerified",
                table: "employees",
                newName: "password_verified");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password_verified",
                table: "employees",
                newName: "PasswordVerified");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 260, DateTimeKind.Utc).AddTicks(731), "$2a$11$pwW4IEEDezHZvSWi29eq2ehdrTsLAXehqEnEHBrBz6LXOP7THWGK2" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(5889), "$2a$11$K8XB2HxZui2j/bHEF1himuHoknSHR5ud4FjUsctv.Imksxsb15l0i" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6827), new DateTime(2024, 5, 1, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6828), new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6828) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6832), new DateTime(2024, 5, 1, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6834), new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6833) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6836), new DateTime(2024, 5, 1, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6838), new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6836) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6840), new DateTime(2024, 5, 1, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6841), new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6840) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6843), new DateTime(2024, 5, 1, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6847), new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6846) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6686), new DateTime(2024, 5, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6693), new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6707) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6712), new DateTime(2024, 6, 23, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6713), new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6715) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6718), new DateTime(2024, 7, 23, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6719), new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6722) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6754));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6761));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6763));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6771));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6774));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6776));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6781));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6782));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(6784));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 92, DateTimeKind.Utc).AddTicks(2876));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 4, 24, 3, 6, 36, 92, DateTimeKind.Utc).AddTicks(2878));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 92, DateTimeKind.Utc).AddTicks(1757), new DateTime(2024, 4, 24, 3, 6, 36, 92, DateTimeKind.Utc).AddTicks(1764) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 92, DateTimeKind.Utc).AddTicks(1771), new DateTime(2024, 4, 24, 3, 6, 36, 92, DateTimeKind.Utc).AddTicks(1772) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 92, DateTimeKind.Utc).AddTicks(1774), new DateTime(2024, 4, 24, 3, 6, 36, 92, DateTimeKind.Utc).AddTicks(1775) });
        }
    }
}
