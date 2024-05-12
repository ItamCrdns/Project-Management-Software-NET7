using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class removeuserrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "addresses",
                keyColumn: "address_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "addresses",
                keyColumn: "address_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "addresses",
                keyColumn: "address_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "employeeissues",
                keyColumn: "relation_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "employeeissues",
                keyColumn: "relation_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "employeeprojects",
                keyColumn: "relation_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "employeeprojects",
                keyColumn: "relation_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "employeeprojects",
                keyColumn: "relation_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "employeetasks",
                keyColumn: "relation_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "employeetasks",
                keyColumn: "relation_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "employeetasks",
                keyColumn: "relation_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "issues",
                keyColumn: "issue_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tasks",
                keyColumn: "task_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "projects",
                keyColumn: "project_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "companies",
                keyColumn: "company_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "companies",
                keyColumn: "company_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "employee_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "companies",
                keyColumn: "company_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tiers",
                keyColumn: "tier_id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "role",
                table: "employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "employees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "addresses",
                columns: new[] { "address_id", "city", "country", "postal_code", "state", "street_address" },
                values: new object[,]
                {
                    { 1, "Faketown", "Fake Country", "12345", "FS", "123 Fake Street" },
                    { 2, "Fakecity", "Fake Country", "67890", "FC", "456 Fake Avenue" },
                    { 3, "Fakeville", "Fake Country", "11122", "FV", "789 Fake Boulevard" }
                });

            migrationBuilder.InsertData(
                table: "companies",
                columns: new[] { "company_id", "added_by_id", "address_id", "ceo_user_id", "contact_email", "contact_phone_number", "latest_project_creation", "logo", "name" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, "fake1@example.com", "1234567890", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Fake Company 1" },
                    { 2, 2, 2, 2, "fake2@example.com", "0987654321", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Fake Company 2" },
                    { 3, 3, 3, 3, "fake3@example.com", "1231231231", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Fake Company 3" }
                });

            migrationBuilder.InsertData(
                table: "tiers",
                columns: new[] { "tier_id", "created", "duty", "name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(521), "Duty for Supervisor", "Supervisor" },
                    { 2, new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(522), "Duty for Employee", "Employee" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "created", "first_name", "gender", "last_login", "last_name", "profile_picture", "username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(192), "First", "Male", new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(195), "User", null, "User1" },
                    { 2, new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(199), "Second", "Female", new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(199), "User", null, "User2" },
                    { 3, new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(201), "Third", "Male", new DateTime(2024, 5, 11, 7, 43, 24, 86, DateTimeKind.Utc).AddTicks(201), "User", null, "User3" }
                });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "employee_id", "company_id", "created", "email", "first_name", "gender", "last_login", "last_name", "lockout_enabled", "locked_until", "login_attempts", "password", "password_verified", "phone_number", "profile_picture", "role", "supervisor_id", "tier_id", "username" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 5, 11, 7, 43, 24, 219, DateTimeKind.Utc).AddTicks(3228), "employee1@example.com", "First", "Male", null, "Employee", false, null, 0, "$2a$11$sfpyC32MytfkU3nPkbRwS.ZGiBkcW0nF6Ctb/Zr/T2CQOjWh7KIzy", null, "1234567890", null, "supervisor", null, 1, "FakeEmployee1" },
                    { 2, 2, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(5639), "employee2@example.com", "Second", "Female", null, "Employee", false, null, 0, "$2a$11$I77JU/WCvj0jB3wsZZvEO.kjucIpmLK04QJ.rnQUXAVRXbFfZVO4i", null, "0987654321", null, "employee", 1, 2, "FakeEmployee2" }
                });

            migrationBuilder.InsertData(
                table: "projects",
                columns: new[] { "project_id", "company_id", "created", "description", "expected_delivery_date", "finished", "latest_task_creation", "lifecycle", "name", "priority", "project_creator_id", "started_working" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6054), "Description for Project 1", new DateTime(2024, 6, 10, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6058), null, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6068), "Planning", "Project 1", 1, 1, null },
                    { 2, 2, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6070), "Description for Project 2", new DateTime(2024, 7, 10, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6070), null, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6074), "Development", "Project 2", 2, 1, null },
                    { 3, 3, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6075), "Description for Project 3", new DateTime(2024, 8, 9, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6075), null, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6076), "Testing", "Project 3", 3, 1, null }
                });

            migrationBuilder.InsertData(
                table: "employeeprojects",
                columns: new[] { "relation_id", "employee_id", "project_id" },
                values: new object[,]
                {
                    { 1, 2, 1 },
                    { 2, 2, 2 },
                    { 3, 2, 3 }
                });

            migrationBuilder.InsertData(
                table: "tasks",
                columns: new[] { "task_id", "created", "description", "expected_delivery_date", "finished", "latest_issue_creation", "name", "project_id", "started_working", "task_creator_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6095), "Description for Task 1", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Task 1", 1, null, 1 },
                    { 2, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6097), "Description for Task 2", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Task 2", 1, null, 1 },
                    { 3, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6098), "Description for Task 3", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Task 3", 1, null, 1 },
                    { 4, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6100), "Description for Task 4", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Task 4", 2, null, 1 },
                    { 5, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6102), "Description for Task 5", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Task 5", 2, null, 1 },
                    { 6, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6103), "Description for Task 6", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Task 6", 2, null, 1 },
                    { 7, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6104), "Description for Task 7", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Task 7", 3, null, 1 },
                    { 8, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6105), "Description for Task 8", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Task 8", 3, null, 1 },
                    { 9, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6108), "Description for Task 9", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Task 9", 3, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "employeetasks",
                columns: new[] { "relation_id", "employee_id", "task_id" },
                values: new object[,]
                {
                    { 1, 2, 4 },
                    { 2, 2, 5 },
                    { 3, 2, 6 }
                });

            migrationBuilder.InsertData(
                table: "issues",
                columns: new[] { "issue_id", "created", "description", "expected_delivery_date", "finished", "issue_creator_id", "name", "started_working", "task_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6135), "Description for Issue 1", null, new DateTime(2024, 5, 18, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6137), 1, "Issue 1", new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6136), 1 },
                    { 2, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6139), "Description for Issue 2", null, new DateTime(2024, 5, 18, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6139), 1, "Issue 2", new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6139), 2 },
                    { 3, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6274), "Description for Issue 3", null, new DateTime(2024, 5, 18, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6275), 1, "Issue 3", new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6274), 4 },
                    { 4, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6276), "Description for Issue 4", null, new DateTime(2024, 5, 18, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6277), 1, "Issue 4", new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6277), 6 },
                    { 5, new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6279), "Description for Issue 5", null, new DateTime(2024, 5, 18, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6280), 1, "Issue 5", new DateTime(2024, 5, 11, 7, 43, 24, 348, DateTimeKind.Utc).AddTicks(6279), 9 }
                });

            migrationBuilder.InsertData(
                table: "employeeissues",
                columns: new[] { "relation_id", "employee_id", "issue_id" },
                values: new object[,]
                {
                    { 1, 2, 4 },
                    { 2, 2, 5 }
                });
        }
    }
}
