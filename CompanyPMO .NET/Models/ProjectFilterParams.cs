namespace CompanyPMO_.NET.Models
{
    public class ProjectFilterParams
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
        public string? Sort { get; set; }
    }
}
