using CompanyPMO_.NET.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("employeeprojects")]
    public class EmployeeProject : IEmployeeEntity
    {
        // Relations table
        [Column("relation_id")]
        public int RelationId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("project_id")]
        public int ProjectId { get; set; }
    }
}
