using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyPMO_.NET.Migrations
{
    /// <inheritdoc />
    public partial class addedentitiesforeignkeystotimeline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "issue_id",
                table: "timelines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "timelines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "task_id",
                table: "timelines",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_timelines_issue_id",
                table: "timelines",
                column: "issue_id");

            migrationBuilder.CreateIndex(
                name: "IX_timelines_project_id",
                table: "timelines",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_timelines_task_id",
                table: "timelines",
                column: "task_id");

            migrationBuilder.AddForeignKey(
                name: "FK_timelines_issues_issue_id",
                table: "timelines",
                column: "issue_id",
                principalTable: "issues",
                principalColumn: "issue_id");

            migrationBuilder.AddForeignKey(
                name: "FK_timelines_projects_project_id",
                table: "timelines",
                column: "project_id",
                principalTable: "projects",
                principalColumn: "project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_timelines_tasks_task_id",
                table: "timelines",
                column: "task_id",
                principalTable: "tasks",
                principalColumn: "task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timelines_issues_issue_id",
                table: "timelines");

            migrationBuilder.DropForeignKey(
                name: "FK_timelines_projects_project_id",
                table: "timelines");

            migrationBuilder.DropForeignKey(
                name: "FK_timelines_tasks_task_id",
                table: "timelines");

            migrationBuilder.DropIndex(
                name: "IX_timelines_issue_id",
                table: "timelines");

            migrationBuilder.DropIndex(
                name: "IX_timelines_project_id",
                table: "timelines");

            migrationBuilder.DropIndex(
                name: "IX_timelines_task_id",
                table: "timelines");

            migrationBuilder.DropColumn(
                name: "issue_id",
                table: "timelines");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "timelines");

            migrationBuilder.DropColumn(
                name: "task_id",
                table: "timelines");
        }
    }
}
