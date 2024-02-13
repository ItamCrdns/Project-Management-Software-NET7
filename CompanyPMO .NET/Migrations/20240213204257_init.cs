using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    address_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    street_address = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    postal_code = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.address_id);
                });

            migrationBuilder.CreateTable(
                name: "changelog",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    entity_id = table.Column<int>(type: "integer", nullable: false),
                    operation = table.Column<string>(type: "text", nullable: false),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    old_data = table.Column<string>(type: "text", nullable: false),
                    new_data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_changelog", x => x.log_id);
                });

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    ceo_user_id = table.Column<int>(type: "integer", nullable: true),
                    address_id = table.Column<int>(type: "integer", nullable: true),
                    contact_email = table.Column<string>(type: "text", nullable: true),
                    contact_phone_number = table.Column<string>(type: "text", nullable: true),
                    added_by_id = table.Column<int>(type: "integer", nullable: true),
                    logo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.company_id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sender_id = table.Column<int>(type: "integer", nullable: true),
                    receiver_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.notification_id);
                });

            migrationBuilder.CreateTable(
                name: "tiers",
                columns: table => new
                {
                    tier_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    duty = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tiers", x => x.tier_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    profile_picture = table.Column<string>(type: "text", nullable: true),
                    last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    profile_picture = table.Column<string>(type: "text", nullable: true),
                    last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    tier_id = table.Column<int>(type: "integer", nullable: false),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    login_attempts = table.Column<int>(type: "integer", nullable: false),
                    locked_until = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    supervisor_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_employees_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employees_employees_supervisor_id",
                        column: x => x.supervisor_id,
                        principalTable: "employees",
                        principalColumn: "employee_id");
                    table.ForeignKey(
                        name: "FK_employees_tiers_tier_id",
                        column: x => x.tier_id,
                        principalTable: "tiers",
                        principalColumn: "tier_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    project_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    finalized = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    project_creator_id = table.Column<int>(type: "integer", nullable: false),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    expected_delivery_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lifecycle = table.Column<string>(type: "text", nullable: true),
                    latest_task_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.project_id);
                    table.ForeignKey(
                        name: "FK_projects_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_projects_employees_project_creator_id",
                        column: x => x.project_creator_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employeeprojects",
                columns: table => new
                {
                    relation_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    project_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeprojects", x => x.relation_id);
                    table.ForeignKey(
                        name: "FK_employeeprojects_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employeeprojects_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    started_working = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    finished = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    task_creator_id = table.Column<int>(type: "integer", nullable: false),
                    project_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.task_id);
                    table.ForeignKey(
                        name: "FK_tasks_employees_task_creator_id",
                        column: x => x.task_creator_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tasks_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employeetasks",
                columns: table => new
                {
                    relation_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    task_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeetasks", x => x.relation_id);
                    table.ForeignKey(
                        name: "FK_employeetasks_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employeetasks_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    image_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    entity_id = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    public_id = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    uploader_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.image_id);
                    table.ForeignKey(
                        name: "FK_images_companies_entity_id",
                        column: x => x.entity_id,
                        principalTable: "companies",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_images_projects_entity_id",
                        column: x => x.entity_id,
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_images_tasks_entity_id",
                        column: x => x.entity_id,
                        principalTable: "tasks",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "issues",
                columns: table => new
                {
                    issue_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    started_working = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    @fixed = table.Column<DateTime>(name: "fixed", type: "timestamp with time zone", nullable: true),
                    issue_creator_id = table.Column<int>(type: "integer", nullable: false),
                    task_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issues", x => x.issue_id);
                    table.ForeignKey(
                        name: "FK_issues_employees_issue_creator_id",
                        column: x => x.issue_creator_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_issues_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employeeissues",
                columns: table => new
                {
                    relation_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    issue_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeissues", x => x.relation_id);
                    table.ForeignKey(
                        name: "FK_employeeissues_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employeeissues_issues_issue_id",
                        column: x => x.issue_id,
                        principalTable: "issues",
                        principalColumn: "issue_id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                columns: new[] { "company_id", "added_by_id", "address_id", "ceo_user_id", "contact_email", "contact_phone_number", "logo", "name" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, "fake1@example.com", "1234567890", null, "Fake Company 1" },
                    { 2, 2, 2, 2, "fake2@example.com", "0987654321", null, "Fake Company 2" },
                    { 3, 3, 3, 3, "fake3@example.com", "1231231231", null, "Fake Company 3" }
                });

            migrationBuilder.InsertData(
                table: "tiers",
                columns: new[] { "tier_id", "created", "duty", "name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2725), "Duty for Supervisor", "Supervisor" },
                    { 2, new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2726), "Duty for Employee", "Employee" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "created", "first_name", "gender", "last_login", "last_name", "profile_picture", "username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2356), "First", "Male", new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2359), "User", null, "User1" },
                    { 2, new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2363), "Second", "Female", new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2363), "User", null, "User2" },
                    { 3, new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2365), "Third", "Male", new DateTime(2024, 2, 13, 20, 42, 56, 662, DateTimeKind.Utc).AddTicks(2365), "User", null, "User3" }
                });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "employee_id", "company_id", "created", "email", "first_name", "gender", "last_login", "last_name", "lockout_enabled", "locked_until", "login_attempts", "password", "phone_number", "profile_picture", "role", "supervisor_id", "tier_id", "username" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 2, 13, 20, 42, 56, 787, DateTimeKind.Utc).AddTicks(5714), "employee1@example.com", "First", "Male", null, "Employee", false, null, 0, "$2a$11$wwfqaUQnEpnmOVDAx2sRY.wW5meZEYfjADufdlZKpG5IG2CjsTQIa", "1234567890", null, "supervisor", null, 1, "FakeEmployee1" },
                    { 2, 2, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4147), "employee2@example.com", "Second", "Female", null, "Employee", false, null, 0, "$2a$11$t49UaYXbcrwHFuqd6Ru61ORES2wB3LXgXVvYB5Gst33gMZo60gbsG", "0987654321", null, "employee", 1, 2, "FakeEmployee2" }
                });

            migrationBuilder.InsertData(
                table: "projects",
                columns: new[] { "project_id", "company_id", "created", "description", "expected_delivery_date", "finalized", "latest_task_creation", "lifecycle", "name", "priority", "project_creator_id" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4561), "Description for Project 1", new DateTime(2024, 3, 14, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4562), null, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4575), "Planning", "Project 1", 1, 1 },
                    { 2, 2, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4576), "Description for Project 2", new DateTime(2024, 4, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4577), null, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4579), "Development", "Project 2", 2, 1 },
                    { 3, 3, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4580), "Description for Project 3", new DateTime(2024, 5, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4580), null, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4581), "Testing", "Project 3", 3, 1 }
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
                columns: new[] { "task_id", "created", "description", "finished", "name", "project_id", "started_working", "task_creator_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4602), "Description for Task 1", null, "Task 1", 1, null, 1 },
                    { 2, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4603), "Description for Task 2", null, "Task 2", 1, null, 1 },
                    { 3, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4605), "Description for Task 3", null, "Task 3", 1, null, 1 },
                    { 4, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4606), "Description for Task 4", null, "Task 4", 2, null, 1 },
                    { 5, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4607), "Description for Task 5", null, "Task 5", 2, null, 1 },
                    { 6, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4608), "Description for Task 6", null, "Task 6", 2, null, 1 },
                    { 7, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4609), "Description for Task 7", null, "Task 7", 3, null, 1 },
                    { 8, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4611), "Description for Task 8", null, "Task 8", 3, null, 1 },
                    { 9, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4612), "Description for Task 9", null, "Task 9", 3, null, 1 }
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
                columns: new[] { "issue_id", "created", "description", "fixed", "issue_creator_id", "name", "started_working", "task_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4638), "Description for Issue 1", new DateTime(2024, 2, 20, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4639), 1, "Issue 1", new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4638), 1 },
                    { 2, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4641), "Description for Issue 2", new DateTime(2024, 2, 20, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4642), 1, "Issue 2", new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4641), 2 },
                    { 3, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4643), "Description for Issue 3", new DateTime(2024, 2, 20, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4644), 1, "Issue 3", new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4643), 4 },
                    { 4, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4645), "Description for Issue 4", new DateTime(2024, 2, 20, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4646), 1, "Issue 4", new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4646), 6 },
                    { 5, new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4647), "Description for Issue 5", new DateTime(2024, 2, 20, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4648), 1, "Issue 5", new DateTime(2024, 2, 13, 20, 42, 56, 923, DateTimeKind.Utc).AddTicks(4648), 9 }
                });

            migrationBuilder.InsertData(
                table: "employeeissues",
                columns: new[] { "relation_id", "employee_id", "issue_id" },
                values: new object[,]
                {
                    { 1, 2, 4 },
                    { 2, 2, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_employeeissues_employee_id",
                table: "employeeissues",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employeeissues_issue_id",
                table: "employeeissues",
                column: "issue_id");

            migrationBuilder.CreateIndex(
                name: "IX_employeeprojects_employee_id",
                table: "employeeprojects",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employeeprojects_project_id",
                table: "employeeprojects",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_company_id",
                table: "employees",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_supervisor_id",
                table: "employees",
                column: "supervisor_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_tier_id",
                table: "employees",
                column: "tier_id");

            migrationBuilder.CreateIndex(
                name: "IX_employeetasks_employee_id",
                table: "employeetasks",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employeetasks_task_id",
                table: "employeetasks",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_images_entity_id",
                table: "images",
                column: "entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_issues_issue_creator_id",
                table: "issues",
                column: "issue_creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_issues_task_id",
                table: "issues",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_company_id",
                table: "projects",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_project_creator_id",
                table: "projects",
                column: "project_creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_project_id",
                table: "tasks",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_task_creator_id",
                table: "tasks",
                column: "task_creator_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "changelog");

            migrationBuilder.DropTable(
                name: "employeeissues");

            migrationBuilder.DropTable(
                name: "employeeprojects");

            migrationBuilder.DropTable(
                name: "employeetasks");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "issues");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "tiers");
        }
    }
}
