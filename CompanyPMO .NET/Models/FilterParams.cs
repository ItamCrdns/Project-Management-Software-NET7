namespace CompanyPMO_.NET.Models
{
    public class FilterParams
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
        public string? Sort { get; set; }
        public string? FilterBy { get; set; }
        public string? FilterValue { get; set; }
        public string? Where { get; set; }
        //public string? WhereValue { get; set; }
    }
}
