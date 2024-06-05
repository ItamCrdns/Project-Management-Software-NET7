using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Dto
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? StartedWorking { get; set; }
        public string? Lifecycle { get; set; }
        public int Priority { get; set; }
        public EmployeeShowcaseDto Creator { get; set; }
        public CompanyShowcaseDto Company { get; set; }
        public int EmployeeCount { get; set; }
        public int TasksCount { get; set; }
        public IEnumerable<EmployeeShowcaseDto>? Team { get; set; }
        public IEnumerable<ProjectPicture>? ImagesCollection { get; set; }
        public List<ProjectPictureDto>? Pictures { get; set; }
    }

    public class ProjectSomeInfoDto
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? StartedWorking { get; set; }
        public DateTime? Finished { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? Lifecycle { get; set; }
        public int Priority { get; set; }
        public EmployeeShowcaseDto Creator { get; set; }
        public IEnumerable<EmployeeShowcaseDto>? Team { get; set; }
        public int EmployeeCount { get; set; }
    }
}
