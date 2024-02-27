namespace CompanyPMO_.NET.Dto
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public PatchEntityImagesDto? Images { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finalized { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? Lifecycle { get; set; }
        public int Priority { get; set; }
        public EmployeeShowcaseDto Creator { get; set; }
        public CompanyShowcaseDto Company { get; set; }
        public int EmployeeCount { get; set; }
        public int TasksCount { get; set; }
        public IEnumerable<EmployeeShowcaseDto>? Team { get; set; }
        public IEnumerable<ImageDto>? ImagesCollection { get; set; }
    }

    public class ProjectSomeInfoDto
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Lifecycle { get; set; }
        public int Priority { get; set; }
        public EmployeeShowcaseDto Creator { get; set; }
        public IEnumerable<EmployeeShowcaseDto>? Team { get; set; }
        public int EmployeeCount { get; set; }
    }
}
