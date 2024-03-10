using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class resetpasswordrequestsguid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResetPasswordRequests_employees_employee_id",
                table: "ResetPasswordRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResetPasswordRequests",
                table: "ResetPasswordRequests");

            migrationBuilder.RenameTable(
                name: "ResetPasswordRequests",
                newName: "resetpasswordrequests");

            migrationBuilder.RenameIndex(
                name: "IX_ResetPasswordRequests_employee_id",
                table: "resetpasswordrequests",
                newName: "IX_resetpasswordrequests_employee_id");

            migrationBuilder.AddColumn<Guid>(
                name: "request_guid",
                table: "resetpasswordrequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_resetpasswordrequests",
                table: "resetpasswordrequests",
                column: "request_id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_resetpasswordrequests_employees_employee_id",
                table: "resetpasswordrequests",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_resetpasswordrequests_employees_employee_id",
                table: "resetpasswordrequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_resetpasswordrequests",
                table: "resetpasswordrequests");

            migrationBuilder.DropColumn(
                name: "request_guid",
                table: "resetpasswordrequests");

            migrationBuilder.RenameTable(
                name: "resetpasswordrequests",
                newName: "ResetPasswordRequests");

            migrationBuilder.RenameIndex(
                name: "IX_resetpasswordrequests_employee_id",
                table: "ResetPasswordRequests",
                newName: "IX_ResetPasswordRequests_employee_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResetPasswordRequests",
                table: "ResetPasswordRequests",
                column: "request_id");

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 112, DateTimeKind.Utc).AddTicks(7188), "$2a$11$Nu.DryNSLwbQsTPUKeYAYuuutEMUp16kNdd8roY1IgYzoh6V/6lbO" });

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2,
                columns: new[] { "created", "password" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5372), "$2a$11$tGb8YWdzjaOhhJ7c422h5OnppcVWr9T9P7JZk6vPmHZkEq0NSrPG2" });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5848), new DateTime(2024, 3, 17, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5850), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5849) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5852), new DateTime(2024, 3, 17, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5854), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5853) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5856), new DateTime(2024, 3, 17, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5857), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5856) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5859), new DateTime(2024, 3, 17, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5860), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5859) });

            migrationBuilder.UpdateData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5,
                columns: new[] { "created", "fixed", "started_working" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5862), new DateTime(2024, 3, 17, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5863), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5862) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5737), new DateTime(2024, 4, 9, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5742), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5753) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5756), new DateTime(2024, 5, 9, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5756), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5759) });

            migrationBuilder.UpdateData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3,
                columns: new[] { "created", "expected_delivery_date", "latest_task_creation" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5760), new DateTime(2024, 6, 8, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5761), new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5762) });

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5796));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5798));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5800));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5802));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5803));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5805));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5809));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5811));

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 57, 239, DateTimeKind.Utc).AddTicks(5814));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(1537));

            migrationBuilder.UpdateData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2,
                column: "created",
                value: new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(1538));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(964), new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(965) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(969), new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(970) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                columns: new[] { "created", "last_login" },
                values: new object[] { new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(971), new DateTime(2024, 3, 10, 1, 28, 56, 988, DateTimeKind.Utc).AddTicks(972) });

            migrationBuilder.AddForeignKey(
                name: "FK_ResetPasswordRequests_employees_employee_id",
                table: "ResetPasswordRequests",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
