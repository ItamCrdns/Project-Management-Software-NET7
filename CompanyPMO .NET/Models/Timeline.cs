using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{

    [Table("timelines")]
    public class Timeline
    {
        [Column("timeline_id")]
        public int TimelineId { get; set; }
        [Column("event")]
        public string Event { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("project_id")]
        public int? ProjectId { get; set; }
        [Column("task_id")]
        public int? TaskId { get; set; }
        [Column("issue_id")]
        public int? IssueId { get; set; }
        [Column("type")]
        public string Type { get; set; }

        public Employee Employee { get; set; }
        public Project? Project { get; set; }
        public Task? Task { get; set; }
        public Issue? Issue { get; set; }

    }
}
