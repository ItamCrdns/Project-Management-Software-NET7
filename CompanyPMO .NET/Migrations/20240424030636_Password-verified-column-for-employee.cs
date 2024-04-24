using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class Passwordverifiedcolumnforemployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordVerified",
                table: "employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password", "PasswordVerified" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 260, DateTimeKind.Utc).AddTicks(731), "$2a$11$pwW4IEEDezHZvSWi29eq2ehdrTsLAXehqEnEHBrBz6LXOP7THWGK2", null });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password", "PasswordVerified" },
                values: new object[] { new DateTime(2024, 4, 24, 3, 6, 36, 433, DateTimeKind.Utc).AddTicks(5889), "$2a$11$K8XB2HxZui2j/bHEF1himuHoknSHR5ud4FjUsctv.Imksxsb15l0i", null });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordVerified",
                table: "employees");

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
    }
}
