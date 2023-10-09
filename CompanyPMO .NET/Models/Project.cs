using CompanyPMO_.NET.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    public class Project
    {
        [Column("project_id")]
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Finalized { get; set;}
        [Column("project_creator_id")]
        public int ProjectCreatorId { get; set; }
        [Column("company_id")]
        public int CompanyId { get; set; }
        public int Priority { get; set; }

        // Navigation properties

        public ICollection<Image>? Images { get; set; }
        public List<Employee>? Employees { get; set; }
        public Company? Company { get; set; }
        public Employee? ProjectCreator { get; set; }
    }
}
