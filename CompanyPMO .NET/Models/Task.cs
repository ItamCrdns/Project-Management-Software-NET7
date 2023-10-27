using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    public class Task
    {
        [Column("task_id")]
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        [Column("started_working")]
        public DateTimeOffset? StartedWorking { get; set; }
        public DateTimeOffset? Finished { get; set; }
        [Column("task_creator_id")]
        public int TaskCreatorId { get; set; }
        [Column("project_id")]
        public int ProjectId { get; set; }

        // Navigation properties

        public ICollection<Image>? Images { get; set; }
        public List<Employee>? Employees { get; set; }
        public Employee? TaskCreator { get; set; }
        public Project Project { get; set; }
    }
}
