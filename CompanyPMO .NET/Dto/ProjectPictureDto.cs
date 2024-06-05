namespace CompanyPMO_.NET.Dto
{
    public class ProjectPictureDto
    {
        public int ProjectPictureId { get; set; }
        public string ImageUrl { get; set; }
        public string CloudinaryPublicId { get; set; }
        public DateTime Created { get; set; }
        public EmployeeShowcaseDto Employee { get; set; }
    }
}
