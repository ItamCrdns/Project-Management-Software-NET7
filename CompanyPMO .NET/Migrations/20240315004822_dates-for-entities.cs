using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class datesforentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "finalized",
                table: "projects",
                newName: "started_working");

            migrationBuilder.RenameColumn(
                name: "fixed",
                table: "issues",
                newName: "finished");

            migrationBuilder.AddColumn<DateTime>(
                name: "expected_delivery_date",
                table: "tasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "finished",
                table: "projects",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expected_delivery_date",
                table: "issues",
                type: "timestamp with time zone",
                nullable: true);

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
                columns: new[] { "created", "expected_delivery_date", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6220), null, new DateTime(2024, 3, 22, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6222), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6221) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6226), null, new DateTime(2024, 3, 22, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6228), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6227) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6230), null, new DateTime(2024, 3, 22, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6234), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6232) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "expected_delivery_date", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6237), null, new DateTime(2024, 3, 22, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6240), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6238) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "expected_delivery_date", "finished", "started_working" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6242), null, new DateTime(2024, 3, 22, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6243), new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6242) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "finished", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5871), new DateTime(2024, 4, 14, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5873), null, new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5890) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "finished", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5894), new DateTime(2024, 5, 14, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5895), null, new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5899) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "finished", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5901), new DateTime(2024, 6, 13, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5902), null, new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5903) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5948), null });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5952), null });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5954), null });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                columns: new[] { "created", "expected_delivery_date" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5956), null });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                columns: new[] { "created", "expected_delivery_date" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(5959), null });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                columns: new[] { "created", "expected_delivery_date" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6166), null });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                columns: new[] { "created", "expected_delivery_date" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6168), null });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                columns: new[] { "created", "expected_delivery_date" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6170), null });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                columns: new[] { "created", "expected_delivery_date" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 48, 22, 447, DateTimeKind.Utc).AddTicks(6173), null });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expected_delivery_date",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "finished",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "expected_delivery_date",
                table: "issues");

            migrationBuilder.RenameColumn(
                name: "started_working",
                table: "projects",
                newName: "finalized");

            migrationBuilder.RenameColumn(
                name: "finished",
                table: "issues",
                newName: "fixed");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 739, DateTimeKind.Utc).AddTicks(1462), "$2a$11$H.5SN.Ug74EmDSAfNPw91OOKMWcPdCdmrO5VRrX.9n79XHBs7HGje" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 863, DateTimeKind.Utc).AddTicks(9993), "$2a$11$qZJtD3FORx0re8DCVCxGrugHdp9elOMPBq7CGSw165G8uZKjOLGti" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(353), new DateTime(2024, 3, 17, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(355), new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(354) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(357), new DateTime(2024, 3, 17, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(357), new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(357) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(359), new DateTime(2024, 3, 17, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(360), new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(359) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(361), new DateTime(2024, 3, 17, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(362), new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(362) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(364), new DateTime(2024, 3, 17, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(364), new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(364) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(283), new DateTime(2024, 4, 9, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(284), new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(293) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(294), new DateTime(2024, 5, 9, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(295), new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(296) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(297), new DateTime(2024, 6, 8, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(298), new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(299) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(319));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(321));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(322));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(323));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(324));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(325));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(326));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(327));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 864, DateTimeKind.Utc).AddTicks(329));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 614, DateTimeKind.Utc).AddTicks(3625));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 10, 4, 8, 51, 614, DateTimeKind.Utc).AddTicks(3627));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 614, DateTimeKind.Utc).AddTicks(3221), new DateTime(2024, 3, 10, 4, 8, 51, 614, DateTimeKind.Utc).AddTicks(3224) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 614, DateTimeKind.Utc).AddTicks(3228), new DateTime(2024, 3, 10, 4, 8, 51, 614, DateTimeKind.Utc).AddTicks(3228) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 10, 4, 8, 51, 614, DateTimeKind.Utc).AddTicks(3230), new DateTime(2024, 3, 10, 4, 8, 51, 614, DateTimeKind.Utc).AddTicks(3230) });
        }
    }
}
