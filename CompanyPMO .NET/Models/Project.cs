using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        // Navigation properties

        public ICollection<Image>? Images { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}
