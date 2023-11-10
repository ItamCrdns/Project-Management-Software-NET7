namespace CompanyPMO_.NET.Models
{
    public class ProjectFilterParams
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
        public bool? Ascending { get; set; }
        public bool? Descending { get; set; }
    }
}
