namespace CompanyPMO_.NET.Models
{
    public class FilterParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? OrderBy { get; set; }
        public string? Sort { get; set; }
        public string? SearchBy { get; set; }
        public string? SearchValue { get; set; }
        public string? FilterBy { get; set; }
        public string? FilterValue { get; set; }
        public string? FilterWhere { get; set; }
        public string? FilterWhereValue { get; set; }
    }
}
