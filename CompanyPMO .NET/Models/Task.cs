using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("tasks")]
    public class Task
    {
        [Column("task_id")]
        public int TaskId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("started_working")]
        public DateTime? StartedWorking { get; set; }
        [Column("finished")]
        public DateTime? Finished { get; set; }
        [Column("expected_delivery_date")]
        public DateTime? ExpectedDeliveryDate { get; set; }
        [Column("task_creator_id")]
        public int TaskCreatorId { get; set; }
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Column("latest_issue_creation")]
        public DateTime LatestIssueCreation { get; set; }

        // Navigation properties

        public ICollection<Image>? Images { get; set; }
        public ICollection<Issue>? Issues { get; set; }
        public List<Employee>? Employees { get; set; }
        public Employee? TaskCreator { get; set; }
        public Project Project { get; set; }
    }
}
