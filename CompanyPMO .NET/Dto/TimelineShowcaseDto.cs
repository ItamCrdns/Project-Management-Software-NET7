namespace CompanyPMO_.NET.Dto
{
    public class TimelineShowcaseDto
    {
        public int TimelineId { get; set; }
        public string Event { get; set; }
        public DateTime Created { get; set; }
        public string Type { get; set; }
        public string EventText { get; set; }
        public EmployeeShowcaseDto Employee { get; set; }
        public ProjectShowcaseDto? Project { get; set; }
        public TaskShowcaseDto? Task { get; set; }
        public IssueShowcaseDto? Issue { get; set; }
    }
}
