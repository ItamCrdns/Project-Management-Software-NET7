namespace CompanyPMO_.NET.Dto
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public PatchEntityImagesDto? Images { get; set; }
    }
}
