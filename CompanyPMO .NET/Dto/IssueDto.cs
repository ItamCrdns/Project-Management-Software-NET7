namespace CompanyPMO_.NET.Dto
{
    public class IssueDto
    {
        public int IssueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? StartedWorking { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? Finished { get; set; }
        public IEnumerable<EmployeeShowcaseDto>? Employees { get; set; }
        public EmployeeShowcaseDto? IssueCreator { get; set; }
        public int EmployeeCount { get; set; }
        public TaskShowcaseDto? Task { get; set; }
    }
}
