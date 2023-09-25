using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    public class EmployeeProject
    {
        // Relations table
        [Column("relation_id")]
        public int RelationId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("project_id")]
        public int ProjectId { get; set; }

        // Navigation properties

        public Employee Employee { get; set; }
        public Project Project { get; set; }
    }
}
