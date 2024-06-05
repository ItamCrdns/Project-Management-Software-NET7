using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("workload")]
    public class Workload
    {
        [Column("workload_id")]
        public int WorkloadId { get; set; }
        [Column("workload_sum")]
        public string? WorkloadSum { get; set; } // Store a string "Low", "Medium", "High" to represent the workload of an employee
        [Column("workload_sum_id")]
        public int WorkloadSumId { get; set; } // Store the id of the workload sum
        [Column("assigned_projects")]
        public int? AssignedProjects { get; set; }
        [Column("completed_projects")]
        public int? CompletedProjects { get; set; }
        [Column("overdue_projects")]
        public int? OverdueProjects { get; set; }
        [Column("created_projects")]
        public int? CreatedProjects { get; set; }
        [Column("assigned_tasks")]
        public int? AssignedTasks { get; set; }
        [Column("completed_tasks")]
        public int? CompletedTasks { get; set; }
        [Column("overdue_tasks")]
        public int? OverdueTasks { get; set; }
        [Column("created_tasks")]
        public int? CreatedTasks { get; set; }
        [Column("assigned_issues")]
        public int? AssignedIssues { get; set; }
        [Column("completed_issues")]
        public int? CompletedIssues { get; set; }
        [Column("overdue_issues")]
        public int? OverdueIssues { get; set; }
        [Column("created_issues")]
        public int? CreatedIssues { get; set; }

        // Navigation properties
        public Employee Employee { get; set; }
    }
}
