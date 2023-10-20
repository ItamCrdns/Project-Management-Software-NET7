using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Dto
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? StartedWorking { get; set; }
        public DateTimeOffset? Finished { get; set; }
    }
}
