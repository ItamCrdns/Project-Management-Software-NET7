namespace CompanyPMO_.NET.Dto
{
    public class IssueShowcaseDto
    {
        public int IssueId { get; set; }
        public string Name { get; set; }
        public bool IsCreator { get; set; }
        public int? ClientId { get; set; }
        public int? TaskId { get; set; }
        public int? ProjectId { get; set; }
    }
}
