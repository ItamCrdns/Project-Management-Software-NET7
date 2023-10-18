namespace CompanyPMO_.NET.Dto
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public PatchEntityImagesDto? Images { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Finalized { get; set; }
        public int Priority { get; set; }
        public EmployeeShowcaseDto ProjectCreator { get; set; }
        public CompanyShowcaseDto Company { get; set; }
        public int EmployeeCount { get; set; }
        public int TasksCount { get; set; }
        public IEnumerable<EmployeeShowcaseDto>? Employees { get; set; }
        public IEnumerable<ImageDto>? ImagesCollection { get; set; }
    }
}
